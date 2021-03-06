﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSharpinator
{
    [DebuggerDisplay("List<{Class.TypeName is BclClass ? ((BclClass)Class).TypeAlias : ((UserDefinedClass)Class).TypeName.Raw}>")]
    public class ListClass : IClass
    {
        private static readonly ConcurrentDictionary<IClass, ListClass> _classes = new ConcurrentDictionary<IClass, ListClass>();
        private readonly IClass _class;

        private ListClass(IClass @class)
        {
            _class = @class;
        }

        public static ListClass FromClass(IClass @class)
        {
            return _classes.GetOrAdd(@class, x => new ListClass(x));
        }

        public IClass Class
        {
            get { return _class; }
        }

        public bool IsPlural { get { return true; } }

        public string GeneratePropertyCode(string propertyName, Case classCase, IEnumerable<AttributeProxy> attributes, DocumentType documentType)
        {
            string typeName;

            var userDefinedClass = _class as UserDefinedClass;
            if (userDefinedClass != null)
            {
                typeName =
                    string.IsNullOrEmpty(userDefinedClass.CustomName)
                        ? userDefinedClass.TypeName.FormatAs(classCase)
                        : userDefinedClass.CustomName;
            }
            else
            {
                typeName = ((IBclClass)_class).TypeAlias;
            }

            var sb = new StringBuilder();

            foreach (var attribute in attributes)
            {
                sb.AppendLine(string.Format("{0}", attribute.ToCode()));
            }

            sb.AppendFormat("public List<{0}> {1} {{ get; set; }}", typeName, propertyName);

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            var otherListClass = other as ListClass;
            if (otherListClass == null)
            {
                return false;
            }

            return Equals(_class, otherListClass._class);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = "ListClass".GetHashCode();
                result = (result * 397) ^ (_class.GetHashCode());
                return result;
            }
        }
    }
}
