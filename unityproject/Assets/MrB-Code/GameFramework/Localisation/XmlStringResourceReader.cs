using System;
using System.Collections.Generic;
using System.Xml;

namespace Assets.GameFramework.Localisation
{
    public class XmlStringResourceReader    
    {
        private readonly IDictionary<string, string> _container;

        public XmlStringResourceReader(IDictionary<string, string> container)
        {
            _container = container;
        }

        public void Read(string text)
        {
            using (var stringReader = new System.IO.StringReader(text))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {
                        switch (xmlReader.Name)
                        {
                            case "resources":
                                break;
                            case "string":
                                var attribute = xmlReader["name"];
                                var hasValue = xmlReader.Read();
                                if (attribute != null && hasValue)
                                    _container.Add(attribute, xmlReader.Value.Trim());
                                break;
                        }
                    }
                }
            }
        }
    }
}