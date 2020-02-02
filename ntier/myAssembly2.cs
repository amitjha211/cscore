using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;

namespace NTier
{
    public class myAssembly2
    {

        private static void setPropertyValue(string sName, string sValue, object obj)
        {

            var p = obj.GetType().GetProperty(sName);
            if (p == null) return;

            if (p.PropertyType == typeof(string))
                p.SetValue(obj, sValue, null);
            else if (p.PropertyType == typeof(int))
                p.SetValue(obj, Convert.ToInt32(sValue), null);
            else if (p.PropertyType == typeof(decimal))
                p.SetValue(obj, Convert.ToDecimal(sValue), null);
            else if (p.PropertyType == typeof(double))
                p.SetValue(obj, Convert.ToDouble(sValue), null);
        }

        private static void setPropertyValue(XmlNodeList nodes, object obj)
        {
            foreach (XmlNode node in nodes)
            {
                setPropertyValue(node, obj);
            }
        }

        private static void setPropertyValue(XmlNode node, object obj)
        {

            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    setPropertyValue(attr.Name, attr.Value, obj);


            var p = obj.GetType().GetProperty(node.Name);

            if (p == null) return;


            if (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text)
            {
                if (p.PropertyType == typeof(string))
                    p.SetValue(obj, node.InnerText, null);
                else if (p.PropertyType == typeof(int))
                    p.SetValue(obj, Convert.ToInt32(node.InnerText), null);
                else if (p.PropertyType == typeof(decimal))
                    p.SetValue(obj, Convert.ToDecimal(node.InnerText), null);
                else if (p.PropertyType == typeof(double))
                    p.SetValue(obj, Convert.ToDouble(node.InnerText), null);
            }
            else
            {
                string sType = node.getXmlAttributeValue("type");

                if (node.Attributes.Count > 0 || node.ChildNodes.Count > 0)
                {
                    var obj2 = p.PropertyType.Assembly.CreateInstance(p.PropertyType.FullName);

                    if (sType == "list")
                    {
                        Type p2 = p.PropertyType.GetGenericArguments()[0];
                        List<object> list_tmp_objects = new List<object>();

                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            var objTmp = p2.Assembly.CreateInstance(p2.FullName);

                            setPropertyValue(node2, objTmp);

                            object[] objParams = new object[] { objTmp };
                            obj2.GetType().GetMethod("Add").Invoke(obj2, objParams);
                            list_tmp_objects.Add(objTmp);

                            if (node2.ChildNodes != null && node2.ChildNodes.Count > 0)
                                setPropertyValue(node2.ChildNodes, objTmp);
                        }


                    }
                    else
                    {
                        foreach (XmlAttribute attr2 in node.Attributes)
                            setPropertyValue(attr2.Name, attr2.Value, obj2);

                        foreach (XmlNode node2 in node.ChildNodes)
                            setPropertyValue(node2, obj2);
                    }

                    p.SetValue(obj, obj2, null);
                }

            }

        }


        //private static void setPropertyValue(NameValueCollection lstProperties, object obj)
        //{
        //    foreach (string sName in lstProperties.AllKeys)
        //    {
        //        setPropertyValue(sName, lstProperties[sName], obj);
        //    }
        //}

        

        
        public static object getObjectFromXml(XmlNode rootNode, Type _typeInfo)
        {

            object _obj = _typeInfo.Assembly.CreateInstance(_typeInfo.FullName); //; AppDomain.CurrentDomain.CreateInstance(asmName, clsPath).Unwrap();
            setPropertyValue(rootNode, _obj);

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                setPropertyValue(node, _obj);
            }

            return _obj;
        }

        

        public static object getObjectFromXml(XmlNode rootNode)
        {

            string assemblyName = rootNode.getXmlAttributeValue("assemblyName");
            string classPath = rootNode.getXmlAttributeValue("classPath");

            object _obj = AppDomain.CurrentDomain.CreateInstance(assemblyName, classPath).Unwrap();

            setPropertyValue(rootNode, _obj);

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                setPropertyValue(node, _obj);
            }

            return _obj;
        }


        public static T getObjectFromXml<T>(XmlNode rootNode)
        {
            Type _typeInfo = typeof(T);
            object _obj = typeof(T).Assembly.CreateInstance(_typeInfo.FullName); //; AppDomain.CurrentDomain.CreateInstance(asmName, clsPath).Unwrap();

            setPropertyValue(rootNode, _obj);

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                setPropertyValue(node, _obj);
            }

            return (T)_obj;
        }


        private class myType
        {
            public string assemblyName, classPath;
        }

        private class myType2
        {
            public myType _type;
        }


        
        
        public static T getObjectFromJSONString<T>(string sJSON)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sJSON);
        }


        private static Type getTypeFromAssemblyName(string sAssemblyName,string sClassPath)
        {
            //var asm  = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(p => p.GetName().Name == sAssemblyName);
            
            //return asm.GetType(sClassPath);

            return Type.GetType(string.Format("{0},{1}",sClassPath,sAssemblyName));
        }

        public static object createObject(string sAssemblyName, string sClassPath)
        {
            return AppDomain.CurrentDomain.CreateInstance(sAssemblyName, sClassPath).Unwrap();
        }
    }
}
