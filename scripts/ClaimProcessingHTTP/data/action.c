Action()
{

	web_add_auto_header("Accept-Encoding", "gzip, deflate");

	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaim\"");

	soap_request("StepName=enterClaim", 
		"URL=http://localhost:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><enterClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claim xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a"
		":adjustedAmount>0</a:adjustedAmount><a:approvalStatus i:nil=\"true\"/>"
		"<a:approved>false</a:approved><a:claimedAmount>24</a:claimedAmount><a"
		":date>2010-01-01T00:00:00</a:date><a:description>premolar seal</a"
		":description><a:firstName>Hercule</a:firstName><a:lastName>Poirot</a"
		":lastName><a:memberId i:nil=\"true\"/><a:reimbursed>false</a"
		":reimbursed><a:socialSecurityNumber>554-98-0001</a"
		":socialSecurityNumber></claim></enterClaim></s:Body></s:Envelope>", 
		"Snapshot=t1.inf", 
		"ResponseParam=response", 
		LAST);

	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaim\"");

	soap_request("StepName=getClaim", 
		"URL=http://localhost:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><getClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claimId xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a:id>10</a:id><"
		"/claimId></getClaim></s:Body></s:Envelope>", 
		"Snapshot=t2.inf", 
		"ResponseParam=response", 
		LAST);

	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/enterClaim\"");

	soap_request("StepName=enterClaim_2", 
		"URL=http://localhost:8102/ServiceSimulation/Demo/"
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

	web_add_header("Content-Type", "text/xml; charset=utf-8");

	web_add_header("SOAPAction", "\"http://hp.com/SOAQ/ServiceSimulation/2010/demo/01/IClaimProcessing/getClaim\"");

	soap_request("StepName=getClaim_2", 
		"URL=http://localhost:8102/ServiceSimulation/Demo/"
		"ClaimProcessingService/ClaimProcessingService", 
		"SOAPEnvelope=<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/"
		"envelope/\"><s:Body><getClaim xmlns=\"http://hp.com/SOAQ/"
		"ServiceSimulation/2010/demo/01\"><claimId xmlns:a=\"http://"
		"schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo\" "
		"xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><a:id>11</a:id><"
		"/claimId></getClaim></s:Body></s:Envelope>", 
		"Snapshot=t4.inf", 
		"ResponseParam=response", 
		LAST);

	return 0;
}