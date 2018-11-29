using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;

namespace winui
{
    public interface iFormNavigator
    {

        void setTier(NTier.Request.iBussinessTier oTier);
        void setMainForm(Form frmMain);
        Form getForm(string sKey
            ,clsCmd cmd);
        void showForm(string sKey
            , clsCmd cmd);
    }
}
