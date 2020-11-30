using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FileWatcherService
{
    class XmlParser<T> : IConfigParser<T> where T : class
    {
        private readonly string xmlPath = null;
        private readonly string xsdPath = null;

        public XmlParser(string xmlPath)
        {
            this.xmlPath = xmlPath;
            if (File.Exists(Path.ChangeExtension(xmlPath, "xsd")))
            {
                xsdPath = Path.ChangeExtension(xmlPath, "xsd");
            }
        }

        public T Parse()
        {
            if (xsdPath != null && !Validate(xmlPath, xsdPath))
            {
                return null;
            }

            try
            {
                XDocument xDocument = XDocument.Load(xmlPath);
                var elements =
                    from element in xDocument.Elements(typeof(T).Name).DescendantsAndSelf()
                    select element;

                string xmlFormat = elements.First().ToString();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                using (TextReader textReader = new StringReader(xmlFormat))
                {
                    return xmlSerializer.Deserialize(textReader) as T;
                }
            }
            catch (Exception ex)
            {
                using (var streamWriter = new StreamWriter(@"C:\Users\polin\source\repos\3_sem\lr2\FileWatcherService\error.txt", true, Encoding.Default))
                {
                    streamWriter.WriteLine("Error of xml file deserialization: " + ex.Message);
                }
                return null;
            }
        }

        private bool Validate(string xmlPath, string xsdPath)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdPath));

                XmlReader xmlReader = XmlReader.Create(xmlPath, settings);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);
                return true;
            }
            catch (Exception ex)
            {
                using (var streamWriter = new StreamWriter(@"C:\Users\polin\source\repos\3_sem\lr2\FileWatcherService\error.txt", true, Encoding.Default))
                {
                    streamWriter.WriteLine("Error of validation: " + ex.Message);
                }
                return false;
            }
        }
    }
}
