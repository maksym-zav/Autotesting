using Logs.TestInfo;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Logs.Loggers
{
    /// <summary>
    /// Logs test execution data to html document
    /// </summary>
    internal class HtmlLogger : ILogger
    {
        const string templateFileName = "HtmlReportTemplate.xslt";
        private string outputFileName = null;
        private XslCompiledTransform xslTransform;

        public HtmlLogger()
        {
            xslTransform = new XslCompiledTransform();
            xslTransform.Load(templateFileName);
        }

        public void Log(object sender, TestRunData data)
        {
            outputFileName ??= $"{data.TestName}_{DateTime.Now:ddMMyyyy_hhmmss}.html";

            // Serialize run data to xml string
            string xml;
            using (var writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestRunData));
                serializer.Serialize(writer, data);
                xml = writer.ToString();
            }

            // Transform xml to html report
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                using (var writer = new XmlTextWriter(outputFileName, null))
                {
                    xslTransform.Transform(reader, writer);
                }
            }
        }
    }
}
