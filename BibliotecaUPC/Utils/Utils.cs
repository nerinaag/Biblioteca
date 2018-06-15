using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BibliotecaUPC.Utils
{
    public static class Utils
    {
        public static ResultsCode GetValueFromDescription<ResultsCode>(string description)
        {
            var type = typeof(ResultsCode);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (ResultsCode)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (ResultsCode)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }
    }
}