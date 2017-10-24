using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.ServiceModel;

namespace HP.SOAQ.ServiceSimulation.Demo {
    public partial class EnterClaimForm : Form {

        private Model model;

        public EnterClaimForm(Model model) {
            this.model = model;
            InitializeComponent();

            fillInputs();
            enablePrevNext();
        }

        private void enablePrevNext() {
            buttonPrev.Enabled = model.hasPrevClaim();
            buttonNext.Enabled = model.hasNextClaim();
        }

        private void fillInputs() {
            Claim claim = model.getCurrentClaim();
            textFirstName.Text = notNull(claim.firstName);
             textLastName.Text= notNull(claim.lastName);
             textSSN.Text= notNull(claim.socialSecurityNumber);
             textDate.Text = notNull(claim.date).ToShortDateString();
             textDescription.Text= notNull(claim.description);
             textAmount.Text = "" + claim.claimedAmount;
        }

        private static string notNull(string s) {
            return (s == null) ? "" : s;
        }

        private static DateTime notNull(DateTime d) {
            return (d == null) ? new DateTime() : d;
        }

        private void buttonEnter_Click(object sender, EventArgs e) {
            buttonEnter.Enabled = false;
            buttonNext.Enabled = false;
            buttonPrev.Enabled = false;
            textResponse.Text = "Sending request...";

            ClaimProcessingClient claimProcessing = new ClaimProcessingClient();

            try {
                // create claim from input
                Claim claim = new Claim();
                claim.firstName = textFirstName.Text;
                claim.lastName = textLastName.Text;
                claim.socialSecurityNumber = textSSN.Text;
                claim.date = DateTime.Parse(textDate.Text);
                claim.description = textDescription.Text;
                claim.claimedAmount = float.Parse(textAmount.Text);

                // set the visual status for case of exception (SOAP fault)
                approvalStatus.Text = "ERROR";
                // approvalStatus.ForeColor = Color.DimGray;
                //approvalStatus.Text = "REJECTED";
                approvalStatus.ForeColor = Color.Red;

                // enter claim using SOAP call
                ClaimId result = claimProcessing.enterClaim(claim);
                textResponse.Text = "Claim #" + result.id + " entered.\r\n";

                // check claim state using SOAP call
                ClaimId claimId = new ClaimId() { id = result.id };
                Claim processed = claimProcessing.getClaim(claimId);
                textResponse.Text += "Claim #" + result.id + ": " + processed.firstName + " " + processed.lastName + ", " + processed.date.ToShortDateString() + ", $" + processed.claimedAmount + ", " + processed.approvalStatus + "\r\n";

                if (processed.approved)
                {
                    approvalStatus.Text = "APPROVED";
                    approvalStatus.ForeColor = Color.Green;

                } 
                else {
                    approvalStatus.Text = "REJECTED";
                    approvalStatus.ForeColor = Color.Red;
                }

                claimProcessing.Close();
            } catch (FaultException<MemberNotFoundFault> ex) {
                textResponse.Text = "Fault: " + ex.Detail.Reason.Replace("\n", "\r\n"); 
            } catch (Exception ex) {
                textResponse.Text = "Exception occured:\r\n" + ex.Message.Replace("\n", "\r\n"); 
            }

            buttonEnter.Enabled = true;
            enablePrevNext();
            buttonNext.Focus();
        }

        private void buttonPrev_Click(object sender, EventArgs e) {
            model.prev();
            fillInputs();
            enablePrevNext();
        }

        private void buttonNext_Click(object sender, EventArgs e) {
            model.next();
            fillInputs();
            enablePrevNext();
            buttonEnter.Focus();
        }

    }
}
