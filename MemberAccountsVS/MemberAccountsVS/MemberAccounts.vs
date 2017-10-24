<?xml version="1.0" encoding="utf-8"?>
<vs:virtualService version="4.0.2114.47514" id="02c08066-0e3b-4510-bbd6-338db5944234" name="MemberAccounts" description="Virtual service using SOAP over HTTP/HTTPS" activeConfiguration="a281c984-9f8b-4ef1-99b6-50bc78515053" nonExistentRealService="false" xmlns:vs="http://hp.com/SOAQ/ServiceVirtualization/2010/">
  <vs:projectId ref="{439B1C5D-6D06-4963-A65E-178B1AD82602}" />
  <vs:projectName>MemberAccountsVS</vs:projectName>
  <vs:serviceDescription ref="6763873f-be1e-4e85-bec4-3b8a8db42261" />
  <vs:virtualEndpoint type="HTTP" address="MemberAccounts" realAddress="http://dockerclient:8101/ServiceSimulation/Demo/MemberAccountsService/MemberAccountsService" isTemporary="false" isDiscovered="false" id="bc29675d-04c6-4f69-8961-0502a80583cb" name=" Endpoint">
    <vs:virtualInputAgent ref="HttpAgent" />
    <vs:virtualOutputAgent ref="HttpAgent" />
    <vs:realInputAgent ref="HttpAgent" />
    <vs:realOutputAgent ref="HttpAgent" />
  </vs:virtualEndpoint>
  <vs:dataModel ref="6fda1c9c-c826-4a56-9411-944d08addb71" />
  <vs:performanceModel ref="5ed63012-dae6-4466-9c01-f30584bdc006" />
  <vs:performanceModel ref="a1b93f02-27d9-473d-ac4f-0feb40cc188c" />
  <vs:performanceModel ref="b2062b91-f435-4576-a1c1-4883d115f8d6" />
  <vs:performanceModel ref="afa5e7ae-1f29-402b-8e20-173ae166d300" />
  <vs:configuration id="a281c984-9f8b-4ef1-99b6-50bc78515053" name="MemberAccounts Configuration">
    <vs:httpAuthentication>None</vs:httpAuthentication>
    <vs:httpAuthenticationAutodetect>True</vs:httpAuthenticationAutodetect>
    <vs:credentialStore id="b96b6cb3-7808-48fb-b66a-7a545828a4ea">
      <vs:credentials />
      <vs:identities />
    </vs:credentialStore>
    <vs:securityConfiguration>
      <credentials>
        <userName value="Identity[0].UsernamePassword" />
        <clientCertificate value="Identity[0].Certificate" />
        <serviceCertificate value="ServiceIdentity.Certificate" />
      </credentials>
      <security />
      <clientSecurity />
      <serviceSecurity />
    </vs:securityConfiguration>
    <vs:messageSchemaLocked>False</vs:messageSchemaLocked>
    <vs:enableTrackLearning>True</vs:enableTrackLearning>
    <vs:logMessages>False</vs:logMessages>
  </vs:configuration>
</vs:virtualService>