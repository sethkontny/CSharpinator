﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CSharpifier
{
    public class NullableFormattedDateTime : BclClassBase
    {
        private readonly string _format;
        private readonly IFactory _factory;

        public NullableFormattedDateTime(string format, IFactory factory)
            : base("NullableFormattedDateTime", "DateTime?", value => { DateTime temp; return value == null || DateTime.TryParseExact(value, format, new CultureInfo("en-US"), DateTimeStyles.None, out temp); })
        {
            _format = format;
            _factory = factory;
        }

        public override bool IsNullable
        {
            get { return true; }
        }

        public string Format
        {
            get { return _format; }
        }

        public override string GeneratePropertyCode(string propertyName, Case classCase, IEnumerable<AttributeProxy> attributes)
        {
            var sb = new StringBuilder();

            sb.AppendFormat(
                @"[XmlIgnore]
public DateTime? {0} {{ get; set; }}", propertyName).AppendLine().AppendLine();

            foreach (var attribute in attributes)
            {
                sb.AppendLine(string.Format("{0}", attribute.ToCode()));
            }

            sb.AppendFormat(
                @"public string {0}String
{{
    get
    {{
        return {0} == null ? null : {0}.Value.ToString(""{1}"", new CultureInfo(""en-US""));
    }}
    set
    {{
        {0} = value == null ? null : (DateTime?)DateTime.ParseExact(value, ""{1}"", new CultureInfo(""en-US""));
    }}
}}", propertyName, Format);

            return sb.ToString();
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }
            return Equals((NullableFormattedDateTime)other);
        }

        protected bool Equals(NullableFormattedDateTime other)
        {
            return base.Equals(other) && string.Equals(_format, other._format);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (_format != null ? _format.GetHashCode() : 0);
            }
        }
    }
}