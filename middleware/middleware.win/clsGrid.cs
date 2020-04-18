using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace middleware.win
{

    public class clsGridActionButtonParameterMap
    {
        public string source, destination;
    }

    public class clsGridActionButton
    {
        public List<clsGridActionButtonParameterMap> colmap;
        public string name, title, formKey;
    }

    public class clsGridBase
    {
        public string name, title, editForm, crud_get, crud_save, crud_delete;

        public List<clsGridCol> cols { get; set; }
        public List<clsGridActionButton> actionButtons { get; set; }
    }

    public class clsGrid : clsGridBase
    {
        public clsAppService2 appService;
        public clsFormService formService;

        public void createCols(DataGridView grd, bool removeCols = true)
        {
            grd.AutoGenerateColumns = false;
            grd.Columns.Clear();
            foreach (clsGridCol col in cols)
            {
                grd.addColumn(col.text, col.field, col.width);
            }
        }

        public void bindGrid(DataGridView grd, clsCmd cmd)
        {
            var _bindingSource = new BindingSource();
            DataTable t = appService.call(this.crud_get, cmd).result as DataTable;
            _bindingSource.DataSource = t;
            createCols(grd);
            grd.DataSource = _bindingSource;
        }


        public void reloadGrid(DataGridView grd)
        {
            var _bindingSource = grd.DataSource as BindingSource;
            DataTable t = appService.call(this.crud_get, new clsCmd()).result as DataTable;
            _bindingSource.DataSource = t;
        }


        frmGrid frm;
        frmGridSelectMultiple _frmSearch;

        //Amit Jha

        public clsAPIResponse getForm(clsCmd cmd)
        {
            if (frm != null && frm.IsDisposed) frm = null;

            if (frm == null)
            {
                frm = new frmGrid();
                frm.gridInfo = this;
            }

            return new clsAPIResponse() { result = frm };
        }

        public clsAPIResponse getSearchForm(clsCmd cmd)
        {
            if (_frmSearch != null && _frmSearch.IsDisposed) _frmSearch = null;

            if (frm == null)
            {
                _frmSearch = new frmGridSelectMultiple();
                _frmSearch.gridInfo = this;
            }
            return new clsAPIResponse() { result = _frmSearch };
        }
    }
}
