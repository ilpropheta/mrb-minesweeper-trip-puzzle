using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Assets.GameFramework.Localisation
{
    internal class XmlStringResourceWriter
    {
        private readonly string _languageName;
        private readonly IDictionary<string, string> _words;

        public XmlStringResourceWriter(string languageName, IDictionary<string, string> words)
        {
            _languageName = languageName;
            _words = words;
        }

        internal void Write()
        {
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Entitize
            };

            using (var writer = XmlWriter.Create("c:\\missing_strings_" + _languageName + ".xml", xmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("resources");

                foreach (var key in _words.Keys.OrderBy(x=>x))
                {
                    writer.WriteStartElement("string");
                    writer.WriteAttributeString("name", key);
                    writer.WriteString(_words[key]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            
        }
    }
}
