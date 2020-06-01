using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Windows.Controls;
using System.Data.Linq;

namespace CRUDDataGridWPF
{
    public partial class Window2 : Window
    {
        #region Constructor
        public Window2()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window2_Loaded);
        }
        #endregion

        #region Read Operation
        void Window2_Loaded(object sender, RoutedEventArgs e)
        {
            cargaDatos();
        }
        private void cargaDatos(string DescFam = "")
        {
            VenusDbDataContext context = new VenusDbDataContext();
            var result = from fam in context.Familia
                         where System.Data.Linq.SqlClient.SqlMethods.Like(fam.xdescripcion, "%" + DescFam + "%")  || DescFam == ""
                         select fam;
            if (result.ToList().Count > 0)
            {
                txtStatus.Text = "Success: Read Operation";
            }
            dgData.ItemsSource = result.ToList();

        }
        #endregion

        #region Create and Update Operation
        private void dgData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                VenusDbDataContext context = new VenusDbDataContext();
                Familia famgrid = e.Row.DataContext as Familia;

                var matchedData = (from famtabla in context.GetTable<Familia>()
                                   where famgrid.xempresa_id == famtabla.xempresa_id
                                         && famgrid.xfamilia_id == famtabla.xfamilia_id
                                   select famtabla).SingleOrDefault();

                if (matchedData == null)
                {
                    Table<Familia> famTable = context.GetTable<Familia>();

                    Familia registroFamilia = new Familia();
                    registroFamilia.xempresa_id = famgrid.xempresa_id;
                    registroFamilia.xfamilia_id = famgrid.xfamilia_id;
                    registroFamilia.xdescripcion = famgrid.xdescripcion;

                    famTable.InsertOnSubmit(registroFamilia);
                    famTable.Context.SubmitChanges();

                    txtStatus.Text = "Success: Data Inserted";
                }
                else
                {
                    matchedData.xdescripcion = famgrid.xdescripcion;
                    context.SubmitChanges();
                    txtStatus.Text = "Success: Data Updated";
                }
            }
        }
        #endregion

        #region Delete Operation
        private void dgData_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VenusDbDataContext context = new VenusDbDataContext();
            Familia RegFamiliaGrid = dgData.SelectedItem as Familia;

            if (RegFamiliaGrid != null)
            {
                var familiaSeleccionada = (from fam in context.GetTable<Familia>()
                                       where fam.xempresa_id == RegFamiliaGrid.xempresa_id
                                         && fam.xfamilia_id == RegFamiliaGrid.xfamilia_id
                                       select fam).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Are You Sure you want to Delete ?",
                        "Confirm Delete !", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        context.Familia.DeleteOnSubmit(familiaSeleccionada);
                        context.SubmitChanges();
                        txtStatus.Text = "Success: Selected Data Deleted.";
                    }
                }
            }
        }
        #endregion

        protected void Filtrar_pulsado(object sender, EventArgs e)
        {
            cargaDatos(this.Desc_Filtro.Text);
            
        }
    }
}
