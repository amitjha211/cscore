using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace winui
{
    public abstract class FileBinderBase
    {
        public abstract string FieldName { get; }
        public abstract bool isFileSelected();
        public abstract FileData getFileData();
    }

    internal class FileBinderImageSelect : FileBinderBase
    { 
        ucFileSelector _uc1 = null;
        public override string FieldName
        {
            get
            {
                return _uc1.FieldName;
            }
            
        }
        public FileBinderImageSelect(ucFileSelector uc1)
        {
            _uc1 = uc1;
        }

        public override FileData getFileData()
        {
            return _uc1.getFileData();
        }

        public override bool isFileSelected()
        {
            return (_uc1.FieldName.isEmpty() == false && _uc1.isFileSelected() == true);
        }
        
    }
}
