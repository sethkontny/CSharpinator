﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpifier
{
    public static class Extensions
    {
        public static string Indent(this string value)
        {
            return string.Join("\r\n", value.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).Select(x => "    " + x));
        }

        public static BclClass AsBclClass(this Class @class)
        {
            return @class.AsClass<BclClass>();
        }

        public static UserDefinedClass AsUserDefinedClass(this Class @class)
        {
            return @class.AsClass<UserDefinedClass>();
        }

        private static TClass AsClass<TClass>(this Class @class)
            where TClass : Class
        {
            var tClass = @class as TClass;
            if (tClass != null)
            {
                return tClass;
            }

            var listClass = @class as ListClass;
            if (listClass != null)
            {
                return listClass.Class.AsClass<TClass>();
            }

            return null;
        }

        public static bool IsBclClass(this Class @class)
        {
            return @class.IsClass<BclClass>();
        }

        public static bool IsUserDefinedClass(this Class @class)
        {
            return @class.IsClass<UserDefinedClass>();
        }

        private static bool IsClass<TClass>(this Class @class)
            where TClass : Class
        {
            if (@class is TClass)
            {
                return true;
            }

            var listClass = @class as ListClass;
            if (listClass != null)
            {
                return listClass.Class.IsClass<TClass>();
            }

            return false;
        }

        public static IEnumerable<UserDefinedClass> GetUsedClasses(this IClassRepository repository)
        {
            return repository.GetAll().First().GetUsedClasses();
        }

        public static IEnumerable<UserDefinedClass> GetUsedClasses(this UserDefinedClass rootClass)
        {
            return GetUserDefinedClassesImpl(rootClass).Distinct();
        }

        private static IEnumerable<UserDefinedClass> GetUserDefinedClassesImpl(UserDefinedClass @class)
        {
            yield return @class;

            foreach (var childClass in @class.Properties
                .Select(x => x.SelectedPropertyDefinition)
                .Where(x => x.Class.IsUserDefinedClass())
                .Select(x => x.Class.AsUserDefinedClass()))
            {
                foreach (var grandchildClass in GetUserDefinedClassesImpl(childClass))
                {
                    yield return grandchildClass;
                }
            }
        }
    }
}