using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace middleware
{

    public class clsJSONTypeDef
    {
        public string name, assemblyName, classPath;
    }

    public class clsJSONParser
    {
        public List<clsJSONTypeDef> lstTypeInformation = new List<clsJSONTypeDef>();

        public void addTypeDefInfo(string sName, string assemblyName, string classPath)
        {
            var f = new clsJSONTypeDef();
            f.name = sName;
            f.assemblyName = assemblyName;
            f.classPath = classPath;
            lstTypeInformation.Add(f);
        }

        public void loadType(string sJSON)
        {
            JObject jsonObject = JObject.Parse(sJSON);
            
            string strTypeJSON = jsonObject["types"].ToString();

            var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<clsJSONTypeDef>>(strTypeJSON);

            foreach (var f in lst)
            {
                lstTypeInformation.Add(f);
            }
        }


        private static void setPropertyValue(object obj, string sPropertyName, object oValue)
        {
            var p = obj.GetType().GetProperty(sPropertyName);
            if (p != null) p.SetValue(obj, oValue, null);


            var f = obj.GetType().GetField(sPropertyName);
            if (f != null) f.SetValue(obj, oValue);

        }

        private static void setPropertyValue(object obj, string sPropertyName, string sValue)
        {
            var p = obj.GetType().GetProperty(sPropertyName);
            if (p != null)
            {
                if (p.PropertyType == typeof(string))
                    p.SetValue(obj, sValue, null);
                if (p.PropertyType == typeof(int))
                    p.SetValue(obj, Convert.ToInt32(sValue), null);
                if (p.PropertyType == typeof(decimal))
                    p.SetValue(obj, Convert.ToDecimal(sValue), null);
                if (p.PropertyType == typeof(double))
                    p.SetValue(obj, Convert.ToDouble(sValue), null);
                if (p.PropertyType == typeof(bool))
                    p.SetValue(obj, Convert.ToBoolean(sValue), null);
            }

            var f = obj.GetType().GetField(sPropertyName);

            if (f != null)
            {
                if (f.FieldType == typeof(string))
                    f.SetValue(obj, sValue);
                if (f.FieldType == typeof(int))
                    f.SetValue(obj, Convert.ToInt32(sValue));
                if (f.FieldType == typeof(decimal))
                    f.SetValue(obj, Convert.ToDecimal(sValue));
                if (f.FieldType == typeof(double))
                    f.SetValue(obj, Convert.ToDouble(sValue));
                if (f.FieldType == typeof(bool))
                    f.SetValue(obj, Convert.ToBoolean(sValue));
            }

        }



        private Type getMemberDeclaringType(object obj, string sMemberName)
        {
            var p = obj.GetType().GetProperty(sMemberName);
            if (p != null) return p.PropertyType;


            var f = obj.GetType().GetField(sMemberName);
            if (f != null) return f.FieldType;


            return null;

        }




        public object createObject(JObject objJSON, Type objType)
        {

            object obj = null;

            string sType = "";

            if (objJSON["$type"] != null)
            {

                

                switch (objJSON["$type"].Type.ToString())
                {
                    case "String":
                        sType = objJSON["$type"].ToString();


                        if (!string.IsNullOrWhiteSpace(sType))
                        {
                            var fType = this.lstTypeInformation.Find(p => p.name == sType);

                            if (fType != null)
                            {
                                obj = g.createObjectFromAssemblyInfo(fType.assemblyName, fType.classPath);
                            }
                        }
                        break;
                    case "Object":
                        string sAssemblyName = objJSON["$type"]["assemblyName"].ToString();
                        string sClassPath = objJSON["$type"]["classPath"].ToString();

                        if (!sAssemblyName.isEmpty() && !sClassPath.isEmpty())
                        {
                            obj = g.createObjectFromAssemblyInfo(sAssemblyName, sClassPath);
                        }
                        break;
                }

            }

            if (obj == null && objType != null)
            {
                obj = Activator.CreateInstance(objType);
            }


            return obj;

        }

        public object getJSONObject(string strJSON
            , Type objType = null)
        {


            JObject objJSON = JObject.Parse(strJSON);
            object obj = createObject(objJSON, objType);

            if (obj == null) return null;

            ///////////////////////////////////////////////////////

            Type memberType = null;
            object objMember = null;
            foreach (var f in objJSON)
            {

                switch (f.Value.Type.ToString())
                {
                    case "String":
                    case "Float":
                    case "Integer":
                    case "Boolean":
                        setPropertyValue(obj, f.Key, f.Value.ToString());
                        break;
                    case "Object":

                        memberType = getMemberDeclaringType(obj, f.Key);

                        if (memberType != null)
                        {
                            objMember = getJSONObject(f.Value.ToString(), memberType);
                            setPropertyValue(obj, f.Key, objMember);
                        }
                        break;
                    case "Array":
                        memberType = getMemberDeclaringType(obj, f.Key);

                        if (memberType != null)
                        {
                            Type memberListType = getMemberDeclaringType(obj, f.Key).GetGenericArguments()[0];
                            objMember = Activator.CreateInstance(memberType);
                            setPropertyValue(obj, f.Key, objMember);

                            JArray arr = f.Value as JArray;
                            object arrayElement = null;

                            foreach (var JObj in arr)
                            {
                                switch (JObj.Type.ToString())
                                {
                                    case "String":
                                        arrayElement = JObj.ToString();
                                        objMember.GetType().GetMethod("Add").Invoke(objMember, new object[] { arrayElement });
                                        break;
                                    case "Object":
                                        arrayElement = getJSONObject(JObj.ToString(), memberListType);
                                        objMember.GetType().GetMethod("Add").Invoke(objMember, new object[] { arrayElement });
                                        break;
                                }

                            }
                        }
                        break;
                }
            }

            return obj;
        }

        //public T parse<T>(string sJSON)
        //{

        //    Type _typeInfo = typeof(T);
        //    T _obj = (T)typeof(T).Assembly.CreateInstance(_typeInfo.FullName);
        //    JObject obj2 = JObject.Parse(sJSON);
        //    parse(obj2, _obj);
        //    return _obj;
        //}

    }
}
