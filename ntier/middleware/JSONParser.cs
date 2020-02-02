using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
namespace middleware
{

    public class jsonConverter
    {

        private static void setPropertyValue(string sPropertyName, object oValue, object obj)
        {
            var p = obj.GetType().GetProperty(sPropertyName);
            if (p == null) return;
            p.SetValue(obj, oValue, null);
        }

        private static void setPropertyValue(string sPropertyName, string sValue, object obj)
        {
            var p = obj.GetType().GetProperty(sPropertyName);

            if (p == null) return;
            if (p.PropertyType == typeof(string))
                p.SetValue(obj, sValue, null);
            if (p.PropertyType == typeof(int))
                p.SetValue(obj, Convert.ToInt32(sValue), null);
            if (p.PropertyType == typeof(decimal))
                p.SetValue(obj, Convert.ToDecimal(sValue), null);
            if (p.PropertyType == typeof(double))
                p.SetValue(obj, Convert.ToDouble(sValue), null);
        }

        public void parse(JObject objJSON
            , object obj)
        {
            foreach (var f in objJSON)
            {
                switch (f.Value.Type.ToString())
                {
                    case "String":
                    case "Float":
                    case "Integer":
                        setPropertyValue(f.Key, f.Value.ToString(), obj);
                        break;
                    
                    
                    
                }
            }
        }

        
        public T parse<T>(string sJSON)
        {

            

            Type _typeInfo = typeof(T);

            T _obj = (T)typeof(T).Assembly.CreateInstance(_typeInfo.FullName);

            JObject obj2 = JObject.Parse(sJSON);

            parse(obj2, _obj);

            return _obj;
        }








    }
}
