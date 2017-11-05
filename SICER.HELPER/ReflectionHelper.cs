using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SICER.HELPER
{
    public static class ReflectionHelper
    {
        public static dynamic ConvertTo(this object a, Type typeOfB, bool convertNullStringToEmptyString = false, Type typeOfA = null)
        {
            var b = Activator.CreateInstance(typeOfB);

            try
            {
                typeOfA = typeOfA ?? a.GetType();

                foreach (var fieldOfA in typeOfA.GetFields())
                {
                    try
                    {
                        var fieldOfB = typeOfB.GetField(fieldOfA.Name);
                        fieldOfB.SetValue(b, fieldOfA.GetValue(a));
                    }
                    catch (Exception ex) { }
                }
                foreach (var propertyOfA in typeOfA.GetProperties())
                {
                    try
                    {
                        var propertyOfB = typeOfB.GetProperty(propertyOfA.Name);
                        propertyOfB.SetValue(b, propertyOfA.GetValue(a));
                    }
                    catch (Exception ex) { }
                }
                if (convertNullStringToEmptyString)
                    ParseAllPropertiesNullStringToEmptyString(typeOfB, ref b);
            }
            catch (Exception ex)
            {
                return b;
            }


            return b;
        }

        private static void ParseAllPropertiesNullStringToEmptyString(Type type, ref dynamic parentModel)
        {
            var model = parentModel;
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(y => y.PropertyType == typeof(String)).ToList().ForEach(z => z.SetValue(model, z.GetValue(model, null) ?? String.Empty));
            parentModel = model;
        }

        public static List<PropertyInfo> GetSAPUserFieldsProperties(Object model)
        {
            List<PropertyInfo> propertiesList = new List<PropertyInfo>();

            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                /*if (property.Name.StartsWith(ConstantHelper.UserFieldNameStarsWith))
                    propertiesList.Add(property);*/
            }

            return propertiesList;
        }

    }
}
