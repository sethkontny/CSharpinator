﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace CSharpifier
{
    public interface IFactory
    {
        XmlDomElement CreateXmlDomElement(XElement element);
        XmlDomAttribute CreateXmlDomAttribute(XAttribute attribute);
        XmlDomText CreateXmlDomText(string value);

        Property CreateProperty(string id, bool isNonEmpty);
        PropertyDefinition CreatePropertyDefinition(IClass @class, string propertyName, bool isLegal, bool isEnabled, params AttributeProxy[] attributes);

        IEnumerable<IBclClass> GetAllBclClasses();
        IBclClass GetBclClassFromTypeName(string typeName);

        FormattedDateTime GetOrCreateFormattedDateTime(string format);

        NullableFormattedDateTime GetOrCreateNullableFormattedDateTime(string format);
    }
}