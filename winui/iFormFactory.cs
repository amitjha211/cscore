using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winui
{
    public interface iFormFactory
    {
        void setTier(NTier.Request.iBussinessTier oTier);
        Form getForm(string sKey);
     
    }
}
