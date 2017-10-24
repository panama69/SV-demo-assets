using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace HP.SOAQ.ServiceSimulation.Demo {

    [ServiceBehavior(Namespace = "http://hp.com/SOAQ/ServiceSimulation/2010/demo/01", ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class Approval : IApproval {

        #region performance settings
        private static readonly int APPROVE_PROC_RESPONSE_TIME = 30;
        private static readonly int APPROVE_PAYMENT_RESPONSE_TIME = 120;
        #endregion

        private readonly Random RNG = new Random(0);

        private readonly IDictionary<long, float> limitByMemderId = new Dictionary<long, float>();
        private readonly IDictionary<long, float> consumedByMemberId = new Dictionary<long, float>();

        private readonly IDictionary<long, float> approvedPaymentsByClaimId = new Dictionary<long, float>();

        public ApprovalResponse approveProcedure(ProcedureRequest request)
        {
            Console.WriteLine("approveProcedure(" + request.customer + ", claim #" + request.claimId + ", $" + request.amount + ") ");

            float limit, consumed;
            if (!limitByMemderId.ContainsKey(request.memberId))
            {
                limit = 100*(1 + RNG.Next(100));
                consumed = 0;
                limitByMemderId[request.memberId] = limit;
                Console.WriteLine("  limit for member #" + request.memberId + " is: " + limit);
            }
            else
            {
                limit = limitByMemderId[request.memberId];
                consumed = consumedByMemberId[request.memberId];
            }

            var response = new ApprovalResponse() { result = ApprovalResult.approved };
            if (consumed + request.amount > limit)
            {
                response.result = ApprovalResult.denied;
                response.description = "Customer limit of $" + limit + " would be overdrawn.";                
            } else
            {
                approvedPaymentsByClaimId[request.claimId] = request.amount;
                consumedByMemberId[request.memberId] = consumed + request.amount;                
            }

            Thread.Sleep(APPROVE_PROC_RESPONSE_TIME);
            Console.WriteLine("  result: " + response);
            return response;
        }

        public virtual ApprovalResponse approvePayment(PaymentRequest request)
        {
            Console.WriteLine("approvePayment(" + request.customer + ", claim #" + request.claimId + ", $" + request.amount + ") ");            
            var response = new ApprovalResponse() { result = ApprovalResult.approved };

            if (!approvedPaymentsByClaimId.ContainsKey(request.claimId))
            {
                response.result = ApprovalResult.denied;
                response.description = "Claimed procedure was not approved.";
            }
            else
            {
                float limit = approvedPaymentsByClaimId[request.claimId];
                if (request.amount > limit)
                {
                    response.result = ApprovalResult.denied;
                    response.description = "Required payment was outside the approved procedure limit.";
                }
            }

            Thread.Sleep(APPROVE_PAYMENT_RESPONSE_TIME);
            Console.WriteLine("  result: " + response);
            return response;
        }
        
    }


}
