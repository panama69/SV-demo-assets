using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace HP.SOAQ.ServiceSimulation.Demo
{
    class PrintSchema
    {

        private static readonly String SCHEMA_FILE = "TIBCOMemberAccountsService.xsd";

        static void xMain(string[] args)
        {

            XsdDataContractExporter exporter = new XsdDataContractExporter();
            
            List<Type> types = new List<Type>();
            types.Add(typeof (IXMLMemberAccountsRequest));
            types.Add(typeof (IXMLMemberAccountsResponse));

            exporter.Export(types);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            using (XmlWriter writer = XmlTextWriter.Create(SCHEMA_FILE, settings))
            {
                foreach (XmlSchema schema in exporter.Schemas.Schemas("http://schemas.datacontract.org/2004/07/HP.SOAQ.ServiceSimulation.Demo"))
                {
                    schema.Write(writer);
                }
                writer.Flush();
                writer.Close();
            }
            Console.Out.WriteLine("XML schema written to " + SCHEMA_FILE);

        }
    }
}
