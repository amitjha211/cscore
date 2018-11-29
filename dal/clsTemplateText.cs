using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class clsTextTemplate
    {
        public char Start_Field { get; set; }
        public char End_Field { get; set; }
        public Func<string, string> convertText;
        public clsTextTemplate()
        {
            Start_Field = '[';
            End_Field = ']';
        }
        public string getString(string sTemplate)
        {




            var arr = sTemplate.ToCharArray();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < arr.Length; i++)
            {

                if (arr[i] == Start_Field)
                {

                    StringBuilder sbField = new StringBuilder();
                    while (i < arr.Length && arr[i] != End_Field)
                    {
                        i++;
                        if (arr[i] != End_Field) sbField.Append(arr[i]);
                    }

                    if (convertText != null)
                    {
                        sb.Append(convertText(sbField.ToString()));
                    }

                }
                else
                {
                    sb.Append(arr[i]);
                }
            }

            return sb.ToString();
        }

    }
}
