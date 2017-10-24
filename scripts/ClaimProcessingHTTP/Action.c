Action()
{

	lr_save_string(lr_get_attrib_string("targetHost"), "targetHost");
	
	web_add_auto_header("Accept-Encoding", "gzip, deflate");

	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaim\"");
	lr_start_transaction("CreateClaim");

	soap_request("StepName=enterClaim", 
		"URL=http://{targetHost}:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><enterClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claim xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a"
		":adjustedAmount>0</a:adjustedAmount><a:approvalStatus i:nil=\"true\"/>"
		"<a:approved>false</a:approved><a:claimedAmount>{amount}</a:claimedAmount><a"
		":date>{date}</a:date><a:description>{description}</a"
		":description><a:firstName>{firstName}</a:firstName><a:lastName>{lastName}</a"
		":lastName><a:memberId i:nil=\"true\"/><a:reimbursed>false</a"
		":reimbursed><a:socialSecurityNumber>{ssn}</a"
		":socialSecurityNumber></claim></enterClaim></s:Body></s:Envelope>", 
		"Snapshot=t1.inf", 
		"ResponseParam=response", 
		LAST);


	lr_end_transaction("CreateClaim", LR_AUTO);

	lr_xml_get_values("XML={response}",
	                  "ValueParam=claimId",
	                  "Query=/Envelope/Body/enterClaimResponse/enterClaimResult/id",
	                  LAST);
	lr_output_message (lr_eval_string("Claim Created claimId = {claimId}"));
	
	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaim\"");

lr_start_transaction("GetClaim");

			soap_request("StepName=getClaim", 
		"URL=http://{targetHost}:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><getClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claimId xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a:id>{claimId}</a:id><"
		"/claimId></getClaim></s:Body></s:Envelope>", 
		"Snapshot=t2.inf", 
		"ResponseParam=response", 
		LAST);

	lr_end_transaction("GetClaim", LR_AUTO);

/*
	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaim\"");

	lr_start_transaction("CreateClaim");

	soap_request("StepName=enterClaim_2", 
		"URL=http://{targetHost}:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><enterClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claim xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a"
		":adjustedAmount>0</a:adjustedAmount><a:approvalStatus i:nil=\"true\"/>"
		"<a:approved>false</a:approved><a:claimedAmount>400</a:claimedAmount><a"
		":date>2010-02-01T00:00:00</a:date><a:description>ear surgery</a"
		":description><a:firstName>Sherlock</a:firstName><a:lastName>Holmes</a"
		":lastName><a:memberId i:nil=\"true\"/><a:reimbursed>false</a"
		":reimbursed><a:socialSecurityNumber>332-10-0002</a"
		":socialSecurityNumber></claim></enterClaim></s:Body></s:Envelope>", 
		"Snapshot=t3.inf", 
		"ResponseParam=response", 
		LAST);


	lr_end_transaction("CreateClaim", LR_AUTO);

	lr_xml_get_values("XML={response}",
	                  "ValueParam=claimId",
	                  "Query=/Envelope/Body/enterClaimResponse/enterClaimResult/id",
	                  LAST);
	lr_output_message (lr_eval_string("Claim Created claimId = {claimId}"));

	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaim\"");

lr_start_transaction("GetClaim");

		
	soap_request("StepName=getClaim_2", 
		"URL=http://{targetHost}:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><getClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claimId xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a:id>{claimId}</a:id><"
		"/claimId></getClaim></s:Body></s:Envelope>", 
		"Snapshot=t4.inf", 
		"ResponseParam=response", 
		LAST);


	lr_end_transaction("GetClaim", LR_AUTO);
	*/

	return 0;
}