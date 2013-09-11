﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CSharpifier
{
    [XmlRoot("UserDefinedClass")]
    public class UserDefinedClassProxy : ClassProxy
    {
        public string TypeName { get; set; }
        public List<PropertyProxy> Properties { get; set; }

        public static UserDefinedClassProxy FromUserDefinedClass(UserDefinedClass userDefinedClass)
        {
            return new UserDefinedClassProxy
            {
                TypeName = userDefinedClass.TypeName.Raw,
                Properties = userDefinedClass.Properties.Select(PropertyProxy.FromProperty).ToList()
            };
        }

        public UserDefinedClass ToUserDefinedClass(IClassRepository classRepository)
        {
            var userDefinedClass = classRepository.GetOrCreate(TypeName);

            foreach (var propertyProxy in Properties)
            {
                userDefinedClass.AddProperty(propertyProxy.ToProperty(classRepository));
            }

            return userDefinedClass;
        }
    }
}
