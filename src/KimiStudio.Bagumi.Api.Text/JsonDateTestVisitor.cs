using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace KimiStudio.Bagumi.Api.Text
{
    public class JsonDateTestVisitor
    {
        public void Visit(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            if (IsEnumerable(data.GetType()))
            {
                var enumerable = data as IEnumerable;
                if (enumerable == null) return;
                foreach (var item in enumerable)
                {
                    if (item == null) continue;
                    Visit(item);
                }
                return;
            }

            var getPropertys =
                data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);
            foreach (var getProperty in getPropertys)
            {
                VisitGetProperty(data, getProperty);
            }
        }

        private static bool IsEnumerable(Type type)
        {
            return type.GetInterfaces().Any(x => x == typeof(IEnumerable));

        }

        private void VisitGetProperty(object data, PropertyInfo propertyInfo)
        {
            if (IsEnumerable(propertyInfo.PropertyType))
            {
                var enumerable = propertyInfo.GetValue(data, null) as IEnumerable;
                if (enumerable == null) return;

                foreach (var item in enumerable)
                {
                    if (item == null) continue;
                    Visit(item);
                }
            }
            if (propertyInfo.PropertyType.Namespace == "KimiStudio.Bagumi.Api.Models")
            {
                var value = propertyInfo.GetValue(data, null);
                Visit(value);
            }
            else
            {
                VisitVaule(data, propertyInfo);
            }

        }

        private void VisitVaule(object data, PropertyInfo propertyInfo)
        {
            var value = propertyInfo.GetValue(data, null);
            Debug.Print("{0}={1}", propertyInfo.Name, value);
        }



    }
}