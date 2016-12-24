﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Parser
{
    public static class ValueRenderer
    {
        public static void AppendValue(StringBuilder sb, object value, bool legacy, string format)
        {

            string stringValue;
            // Shortcut common case. It is important to do this before IEnumerable, as string _is_ IEnumerable
            if ((stringValue = value as string) != null)
            {
                AppendValueAsString(sb, stringValue, legacy, format);
                return;
            }


            IEnumerable collection;
            if (!legacy && (collection = value as IEnumerable) != null)
            {
                bool separator = false;
                foreach (var item in collection)
                {
                    if (separator) sb.Append(", ");
                    AppendValue(sb, item, false, format);
                    separator = true;
                }
                return;
            }

            IFormattable formattable;
            if (format != null && (formattable = value as IFormattable) != null)
            {
                sb.Append(formattable.ToString(format, CultureInfo.CurrentCulture));
            }

            else if (value is char)
            {
                if (legacy || format == "l")
                    sb.Append((char)value);
                else
                    sb.Append('"').Append((char)value).Append('"');
            }
            else
            {
                sb.Append(value.ToString());
            }
        }

        private static void AppendValueAsString(StringBuilder sb, string stringValue, bool legacy, string format)
        {
            if (legacy || format == "l")
                sb.Append(stringValue);
            else
                sb.Append('"').Append(stringValue).Append('"');
        }
    }
}
