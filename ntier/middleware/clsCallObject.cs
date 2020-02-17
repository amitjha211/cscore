using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace middleware
{

    public class clsCallObject
    {
        public string method;
        public object obj;

        private System.Reflection.MethodInfo funcMethod;

        public void compile()
        {
            funcMethod = obj.GetType().GetMethod(method);
        }

        public clsAPIResponse call(clsCmd cmd)
        {
            return funcMethod.Invoke(obj, new object[] { cmd }) as clsAPIResponse;
        }
    }

    public class clsCallObjects : List<clsCallObject>
    {

        public string path;

        public void compile()
        {
            foreach (clsCallObject f in this)
            {
                f.compile();
            }
        }

        public clsCallObject Add(object objMethod,string sMethod)
        {
            clsCallObject _objCall = new clsCallObject();
            _objCall.method = sMethod;
            _objCall.obj = objMethod;

            this.Add(_objCall);

            return _objCall;
        }

        public clsAPIResponse call(clsCmd cmd)
        {
            clsAPIResponse oLastResponse = new clsAPIResponse() { message=""};
            foreach (clsCallObject f in this.Reverse<clsCallObject>())
            {
                var res = f.call(cmd);
                oLastResponse = res;
                if (res.isValid == false) return res;
            }
            return oLastResponse;
        }
    }

    

}
