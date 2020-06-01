using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//using Microsoft.Windows.Controls;
using System.Data.Linq;

using System.Windows.Threading;
using System.Net;
using System.Net.NetworkInformation;
using CRUDDataGridWPF.Helpers;
using CRUDDataGridWPF.Data;


namespace CRUDDataGridWPF
{
    /// <summary>
    /// Lógica de interacción para PiezasMalas.xaml
    /// </summary>
    public partial class PiezasMalas : Window
    {
        private ICommand _filtrarCMD;
        string fIni_Filtro = "";
        string fFin_Filtro = "";
        string oper_Filtro = "";
        string oper_Filtrocb = "";
        string maquina_Filtrocb = "";
        string refer_Filtro = "";
        string oFOT_Filtro = "";
        string operacion_Filtro = "";
        string enIFS_Filtro = "";
        string cantidad_Filtro = "";
        string udMed_Filtro = "";
        string incid_Filtro = "";
        string tipoPza_Filtro = "";
        string tipoMot_Filtrocb = "";
        string motivo_Filtrocb = "";
        string motivo_malas = "";



        #region Constructor

        public PiezasMalas()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Ventana_Loaded);
        }
        #endregion

        #region Leeemos los registros con filtro
        void Ventana_Loaded(object sender, RoutedEventArgs e)
        {
            cargaCombos();
            cargaDatos();
           
        }
        private void cargaDatos(string oper_Filtro = ""
                                , string maq_Filtro = ""
                                , string fecIni_Filtro = ""
                                , string fecFin_Filtro = ""
                                , string incid_Filtro = ""
                                , string ref_Filtro = ""
                                , string mot_Filtro = ""
                                , string oFOT_Filtro = ""
                                , string operacion_Filtro = ""
                                , string enIFS_Filtro = ""
                                , string cantidad_Filtro = ""
                                , string udMed_Filtro = ""
                                , string tipoPza_Filtro = ""
                                , string tipoMot_Filtrocb = ""
                                )
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            DateTime fecIni;
            DateTime fecFin;
            decimal cantidad_Filtro_decimal=0 ;
            bool enIFS_Filtro_bool = false;

            if (cantidad_Filtro != "")
                cantidad_Filtro_decimal = decimal.Parse(cantidad_Filtro);
            if (enIFS_Filtro != "")
                enIFS_Filtro_bool = bool.Parse(enIFS_Filtro); 

            if (fecIni_Filtro == "")
                fecIni = DateTime.Parse("01/01/2000");
            else
                fecIni = DateTime.Parse(fecIni_Filtro);
            if (fecFin_Filtro == "")
                fecFin = DateTime.Parse("01/01/2100");
            else
                fecFin = DateTime.Parse(fecFin_Filtro);

            /*
            var ListaFichajeCNQ = from e in dcOlanetContext.aperh_PiezasMalas
                                  where (System.Data.Linq.SqlClient.SqlMethods.Like(e.operario_id, "%" + oper_Filtro + "%") || oper_Filtro == "")
                                        && (System.Data.Linq.SqlClient.SqlMethods.Like(e.maquina_id, "%" + maq_Filtro + "%") || maq_Filtro == "")
                                        && (System.Data.Linq.SqlClient.SqlMethods.Like(e.Referencia, "%" + ref_Filtro + "%") || ref_Filtro == "")
                                        && (System.Data.Linq.SqlClient.SqlMethods.Like(e.Incidente, "%" + incid_Filtro + "%") || incid_Filtro == "")
                                        && (System.Data.Linq.SqlClient.SqlMethods.Like(e.TipoMotivo, "%" + mot_Filtro + "%") || mot_Filtro == "")
                                        && (System.Data.Linq.SqlClient.SqlMethods.Like(e.OT, "%" + oFOT_Filtro + "%") || oFOT_Filtro == "")
                                        && (e.FECHA >= fecIni && e.FECHA <= fecFin)
                                  orderby e.IdMalas
                                  select e;
            */


            var ListaFichajeCNQ = from Fichaje in dcOlanetContext.FichajeCNQ
                                   where (Fichaje.operario_id == oper_Filtro || oper_Filtro == "")
                                    && (Fichaje.maquina_id == maq_Filtro || maq_Filtro == "")
                                    && (Fichaje.Referencia == ref_Filtro || ref_Filtro == "")
                                    && (Fichaje.Incidente == incid_Filtro || incid_Filtro == "")
                                    && (Fichaje.IdMotivoMalas.ToString() == mot_Filtro || mot_Filtro == "")
                                    && (Fichaje.OT == oFOT_Filtro || oFOT_Filtro == "")
                                    && (Fichaje.Operacion == operacion_Filtro || operacion_Filtro == "")
                                    && (Fichaje.FECHA >= fecIni && Fichaje.FECHA <= fecFin)
                                    //&& (e.EstaEnIFS.ToString() == enIFS_Filtro || enIFS_Filtro == "")
                                    && (Fichaje.EstaEnIFS == enIFS_Filtro_bool || enIFS_Filtro == "")
                                    && (Fichaje.Cantidad == cantidad_Filtro_decimal || cantidad_Filtro == "")
                                    && (Fichaje.UdMedida == udMed_Filtro || udMed_Filtro == "")
                                    && (Fichaje.Incidente == incid_Filtro || incid_Filtro == "")
                                    && (Fichaje.TipoPieza == tipoPza_Filtro || tipoPza_Filtro == "")
                                    && (Fichaje.IdTipoMotivo.ToString() == tipoMot_Filtrocb || tipoMot_Filtrocb == "")

                                   orderby Fichaje.IdMalas
                                   select Fichaje ;
            //select new { Fichaje}).ToList();
            //select new { e, op.operario_nme };
            //select new { e ,op.operario_nme};
            
            
                   /*     var ListaFichajeCNQ = from e in dcOlanetContext.FichajeCNQ
                                              where (e.operario_id == oper_Filtro || oper_Filtro == "")
                                                    && (e.maquina_id == maq_Filtro || maq_Filtro == "")
                                                    && (e.Referencia == ref_Filtro || ref_Filtro == "")
                                                    && (e.Incidente == incid_Filtro || incid_Filtro == "")
                                                    && (e.IdMotivoMalas.ToString() == mot_Filtro || mot_Filtro == "")
                                                    && (e.OT == oFOT_Filtro || oFOT_Filtro == "")
                                                    && (e.Operacion == operacion_Filtro || operacion_Filtro == "")
                                                    && (e.FECHA >= fecIni && e.FECHA <= fecFin)
                                                    //&& (e.EstaEnIFS.ToString() == enIFS_Filtro || enIFS_Filtro == "")
                                                    && (e.EstaEnIFS == enIFS_Filtro_bool || enIFS_Filtro == "")
                                                    && (e.Cantidad == cantidad_Filtro_decimal || cantidad_Filtro == "")
                                                    && (e.UdMedida == udMed_Filtro || udMed_Filtro == "")
                                                    && (e.Incidente == incid_Filtro || incid_Filtro == "")
                                                    && (e.TipoPieza == tipoPza_Filtro || tipoPza_Filtro == "")
                                                    && (e.TipoMotivo == tipoMot_Filtrocb || tipoMot_Filtrocb == "")

                                              orderby e.IdMalas
                                              select e;
                                              */
            //hemos encotrado datos
            //if (ListaFichajeCNQ.ToList().Count > 0)
                //{
                //    txtStatus.Text = "Existo: Se han encontrado Datos";
                //}
                //dgFichajesCNQ.ItemsSource = ListaFichajeCNQ.ToList();
                //dgFichajesCNQ.ItemsSource = ListaFichajeCNQ;

              //  dgFichajesCNQ.ItemsSource = ListaFichajeCNQ;


            dgFichajesCNQ.ItemsSource = ListaFichajeCNQ;
        }
        #endregion
        #region
        private void cargaCombos()
        {
            cargaComboBoxOperario();
            //cargaComboBoxOperarioPM();
            cargaComboBoxMaquina();
            cargaComboBoxMotivos();
            cargaComboBoxOFOT();
            cargaComboBoxTipoMotivo();
        }

        private void cargaComboBoxOperario()
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            var ListaOperarios = from e in dcOlanetContext.Operario
                                 orderby e.operario_nme
                                 select e;

            cbOperarios_.ItemsSource = ListaOperarios.ToList();
            cbOperarios_.DisplayMemberPath = "operario_nme";
            cbOperarios_.SelectedValuePath = "operario_id";

        }
        /*
        private void cargaComboBoxOperarioPM()
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            var ListaOperariosPM = from e in dcOlanetContext.FichCNQOper
                                 orderby e.operario_nme
                                  select e;

            cbOperarios_PM_.ItemsSource = ListaOperariosPM.ToList();
            cbOperarios_PM_.DisplayMemberPath = "operario_nme";
            cbOperarios_PM_.SelectedValuePath = "operario_id";

        }
        */
        private void cargaComboBoxMaquina()
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            var ListaMaquinas = from e in dcOlanetContext.FichCNQMaq
                                 orderby e.maquina_id
                                 select e;

            cbMaquinas_.ItemsSource = ListaMaquinas.ToList();
            cbMaquinas_.DisplayMemberPath = "maquina_id";
            cbMaquinas_.SelectedValuePath = "maquina_id";

        }

        
        private void cargaComboBoxOFOT()
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            var ListaOFOT = from e in dcOlanetContext.FichCNQOFOT
                               orderby e.OT
                               select e;

            cbOFOT_.ItemsSource = ListaOFOT.ToList();
            cbOFOT_.DisplayMemberPath = "OT";
            cbOFOT_.SelectedValuePath = "OT";

        }
        private void cargaComboBoxMotivos()
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            var ListaMotivos = from e in dcOlanetContext.FichCNQMotivosMalas
                               orderby e.DescMotivoMalas
                               select e;

            cbMotivo_.ItemsSource = ListaMotivos.ToList();
            cbMotivo_.DisplayMemberPath = "DescMotivoMalas";
            cbMotivo_.SelectedValuePath = "IdMotivoMalas";
        }
        private void cargaComboBoxTipoMotivo()
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            var ListaTipoMotivos = from e in dcOlanetContext.FichCNQTiposMotivosMalas
                            orderby e.IdTipoMotivoMalas
                            select e;

            cbTipoMotivo_.ItemsSource = ListaTipoMotivos.ToList();
            cbTipoMotivo_.DisplayMemberPath = "DescTipoMotivoMalas";
            cbTipoMotivo_.SelectedValuePath = "IdTipoMotivoMalas";
      
        }
        #endregion
        #region Insertar y actualizar datos
        private void dzzgData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e) { }


        private void dgData_RowEditEnding_xx(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == Microsoft.Windows.Controls.DataGridEditAction.Commit)
            {
                OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();

                FichajeCNQ datosEnGrid = e.Row.DataContext as FichajeCNQ;

                var datosTabla = (from datostabla in dcOlanetContext.GetTable<FichajeCNQ>()
                                   where datostabla.IdMalas == datosEnGrid.IdMalas
                                   select datostabla).SingleOrDefault();

                if (datosTabla == null)
                {
                    Table<FichajeCNQ> fichCNQTabla = dcOlanetContext.GetTable<FichajeCNQ>();

                    FichajeCNQ registroTabla = new FichajeCNQ();
                    registroTabla.IdMalas = datosEnGrid.IdMalas;
                    registroTabla.maquina_id = datosEnGrid.maquina_id;
                    registroTabla.operario_id = datosEnGrid.operario_id;
                    registroTabla.Referencia = datosEnGrid.Referencia;
                    registroTabla.FECHA = datosEnGrid.FECHA;
                    registroTabla.Cantidad = datosEnGrid.Cantidad;
                    registroTabla.EstaEnIFS = datosEnGrid.EstaEnIFS;
                    registroTabla.OT = datosEnGrid.OT;
                    registroTabla.Operacion = datosEnGrid.Operacion;
                    registroTabla.UdMedida = datosEnGrid.UdMedida;
                    registroTabla.Incidente = datosEnGrid.Incidente;
                    registroTabla.TipoPieza = datosEnGrid.TipoPieza;
                    registroTabla.TipoMotivo = datosEnGrid.TipoMotivo;
                    


                    fichCNQTabla.InsertOnSubmit(registroTabla);
                    fichCNQTabla.Context.SubmitChanges();
                    

                    txtStatus.Text = "Existo: Datos Insertados";
                }
                else
                {
                    datosTabla.maquina_id = datosEnGrid.maquina_id;
                    datosTabla.operario_id = datosEnGrid.operario_id;
                    datosTabla.Referencia = datosEnGrid.Referencia;
                    datosTabla.FECHA = datosEnGrid.FECHA;
                    datosTabla.Cantidad = datosEnGrid.Cantidad;
                    datosTabla.EstaEnIFS = datosEnGrid.EstaEnIFS;
                    datosTabla.OT = datosEnGrid.OT;
                    datosTabla.Operacion = datosEnGrid.Operacion;
                    datosTabla.UdMedida = datosEnGrid.UdMedida;
                    datosTabla.Incidente = datosEnGrid.Incidente;
                    datosTabla.TipoPieza = datosEnGrid.TipoPieza;
                    datosTabla.TipoMotivo = datosEnGrid.TipoMotivo;

                    

                    dcOlanetContext.SubmitChanges();
                    //var datosTablaConsulta = (from datostablaConsulta in dcOlanetContext.GetTable<FichajeCNQ>()
                    //                  where datostabla.IdMalas == datosEnGrid.IdMalas
                    //                  select datostabla).SingleOrDefault();
                    txtStatus.Text = "Exito: Datos Actualizados";
                }
            }
        }

        private void dgData_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == Microsoft.Windows.Controls.DataGridEditAction.Commit)
            {
                OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();

                //aperh_PiezasMalas datosEnGrid = e.Row.DataContext as aperh_PiezasMalas;
                FichajeCNQ datosEnGrid = e.Row.DataContext as FichajeCNQ;

                //DataRowView item = (dgFichajesCNQ as DataGrid).SelectedItem as DataRowView;
                //DataRowView view = (DataRowView)dgFichajesCNQ.SelectedItem;
                /*
                 _contacto.Nombre = item.Row.ItemArray[0].ToString();
                _contacto.NumeroCelular = item.Row.ItemArray[1].ToString();
                _contacto.NumeroTrabajo = item.Row.ItemArray[2].ToString();
                _contacto.NumeroParticular = item.Row.ItemArray[3].ToString();
                _contacto.Email = item.Row.ItemArray[4].ToString();
                _contacto.Notas = item.Row.ItemArray[5].ToString();
                */

                var datosTabla = (from datostabla in dcOlanetContext.GetTable<aperh_PiezasMalas>()
                                  where datostabla.IdMalas == datosEnGrid.IdMalas
                                  select datostabla).SingleOrDefault();

                if (datosTabla == null)
                {
                    Table<aperh_PiezasMalas> fichCNQTabla = dcOlanetContext.GetTable<aperh_PiezasMalas>();

                    aperh_PiezasMalas registroTabla = new aperh_PiezasMalas();
                    registroTabla.IdMalas = datosEnGrid.IdMalas;
                    registroTabla.maquina_id = datosEnGrid.maquina_id;
                    registroTabla.operario_id = datosEnGrid.operario_id;
                    registroTabla.Referencia = datosEnGrid.Referencia;
                    registroTabla.FECHA = datosEnGrid.FECHA;
                    registroTabla.Cantidad = datosEnGrid.Cantidad;
                    registroTabla.EstaEnIFS = datosEnGrid.EstaEnIFS;
                    registroTabla.OT = datosEnGrid.OT;
                    registroTabla.Operacion = datosEnGrid.Operacion;
                    registroTabla.UdMedida = datosEnGrid.UdMedida;
                    registroTabla.Incidente = datosEnGrid.Incidente;
                    registroTabla.TipoPieza = datosEnGrid.TipoPieza;
                    registroTabla.TipoMotivo = datosEnGrid.TipoMotivo;



                    fichCNQTabla.InsertOnSubmit(registroTabla);
                    fichCNQTabla.Context.SubmitChanges();


                    txtStatus.Text = "Existo: Datos Insertados";
                }
                else
                {
                    datosTabla.maquina_id = datosEnGrid.maquina_id;
                    datosTabla.operario_id = datosEnGrid.operario_id;
                    datosTabla.Referencia = datosEnGrid.Referencia;
                    datosTabla.FECHA = datosEnGrid.FECHA;
                    datosTabla.Cantidad = datosEnGrid.Cantidad;
                    datosTabla.EstaEnIFS = datosEnGrid.EstaEnIFS;
                    datosTabla.OT = datosEnGrid.OT;
                    datosTabla.Operacion = datosEnGrid.Operacion;
                    datosTabla.UdMedida = datosEnGrid.UdMedida;
                    datosTabla.Incidente = datosEnGrid.Incidente;
                    datosTabla.TipoPieza = datosEnGrid.TipoPieza;
                    datosTabla.IdTipoMotivo = datosEnGrid.IdTipoMotivo;
                    datosTabla.IdMotivoMalas = datosEnGrid.IdMotivoMalas;
                    
                     dcOlanetContext.SubmitChanges();

                    var datosTablaCons = (from datostablaCons in dcOlanetContext.GetTable<FichajeCNQ>()
                                      where datostablaCons.IdMalas == datosEnGrid.IdMalas
                                      select datostablaCons).SingleOrDefault();
                    
                    datosEnGrid.operario_nme = datosTablaCons.operario_nme;
                    datosEnGrid.DescTipoMotivoMalas = datosTablaCons.DescTipoMotivoMalas;
                    datosEnGrid.DescMotivoMalas = datosTablaCons.DescMotivoMalas;
                    txtStatus.Text = "Exito: Datos Actualizados";
                }
            }
        }

        #endregion
        private void dg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
            
        {
            string a = "AA";
            IList<DataGridCellInfo> celdas = e.AddedCells;
            string valor = celdas[0].Item as string;
        }
        
        #region Delete Operation
        private void dgData_PreviewExecuted_xx(object sender, ExecutedRoutedEventArgs e)
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            FichajeCNQ datosEnGrid = dgFichajesCNQ.SelectedItem as FichajeCNQ;

            if (datosEnGrid != null)
            {
                var registroSeleccionado = (from datostabla in dcOlanetContext.GetTable<FichajeCNQ>()
                                            where datostabla.IdMalas == datosEnGrid.IdMalas
                                            select datostabla).SingleOrDefault();
                if (e.Command == System.Windows.Controls.DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("¿Esta seguro de querer Borrarlo?",
                        "Confirm Delete !", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        dcOlanetContext.FichajeCNQ.DeleteOnSubmit(registroSeleccionado);
                        dcOlanetContext.SubmitChanges();
                        txtStatus.Text = "Exito: Datos Borrados.";
                    }
                }
            }
        }
        private void dgData_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OlanetV5DataContext dcOlanetContext = new OlanetV5DataContext();
            aperh_PiezasMalas datosEnGrid = dgFichajesCNQ.SelectedItem as aperh_PiezasMalas;

            if (datosEnGrid != null)
                {
                    var registroSeleccionado = (from datostabla in dcOlanetContext.GetTable<aperh_PiezasMalas>()
                                                where datostabla.IdMalas == datosEnGrid.IdMalas
                                                select datostabla).SingleOrDefault();
                    if (e.Command == System.Windows.Controls.DataGrid.DeleteCommand)
                    {
                        if (!(MessageBox.Show("¿Esta seguro de querer Borrarlo?",
                            "Confirm Delete !", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            dcOlanetContext.aperh_PiezasMalas.DeleteOnSubmit(registroSeleccionado);
                            dcOlanetContext.SubmitChanges();
                            txtStatus.Text = "Exito: Datos Borrados.";
                        }
                    }
                }
            
        }
        #endregion

        public ICommand AvisoRealizadoCMD
        {
            get
            {
                if (_filtrarCMD == null)
                {
                    _filtrarCMD = new RelayCommand(
                        param => Filtrar());
                }
                return _filtrarCMD;
            }
        }

        private void RealizarAviso()
        {
            //Realizamos el aviso
            //string mensaje ="";
            //_dcOlanet.aperA_Aviso_ActualAHisto(FichajeCNQSeleccionado.MalasId, "CARRETILLA", ref mensaje);
            //FichajeCNQSeleccionado = null;
            //CargarFichajeCNQEnDG("01089");


            //CargarFichajeCNQEnDG();
        }
         protected void Filtrar()
        {
            cargaDatos(oper_Filtro + oper_Filtrocb
                        , maquina_Filtrocb
                        , fIni_Filtro
                        , fFin_Filtro
                        , incid_Filtro
                        , refer_Filtro
                        , motivo_Filtrocb
                        , oFOT_Filtro
                        , operacion_Filtro
                        , enIFS_Filtro
                        , cantidad_Filtro
                        , udMed_Filtro
                        , tipoPza_Filtro
                        , tipoMot_Filtrocb
                        );
                      
        }

        private void Filtrar_Pulsado(object sender, RoutedEventArgs e)
        {
            Filtrar();
        }
        private void BorrarFiltro_Pulsado(object sender, RoutedEventArgs e)
        {
            fIni_Filtro = "";
            fFin_Filtro = "";
            oper_Filtro = "";
            oper_Filtrocb = "";
            maquina_Filtrocb = "";
            refer_Filtro = "";
            oFOT_Filtro = "";
            operacion_Filtro = "";
            refer_Filtro = "";
            enIFS_Filtro = "";
            cantidad_Filtro = "";
            udMed_Filtro = "";
            incid_Filtro = "";
            tipoPza_Filtro = "";
            tipoMot_Filtrocb = "";

            Filtrar();
        }
        
        private void Oper_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {

            ObtValorString(ref oper_Filtro, sender);
        }
        private void CbOperarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtValorcb(ref  oper_Filtrocb, sender);
            
        }
        private void CbMaquinas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtValorcb(ref maquina_Filtrocb, sender);

        }
        private void CbTipoMotivo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtValorcb(ref tipoMot_Filtrocb, sender);

        }

        private void CbMotivo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObtValorcb(ref motivo_Filtrocb, sender);

        }
         private void Refer_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref refer_Filtro, sender); 
        }
        private void Cantidad_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref cantidad_Filtro, sender);
        }

        private void OFOT_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref oFOT_Filtro, sender);
        }

        private void Operacion_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref operacion_Filtro, sender);
   
        }
        private void UdMed_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref udMed_Filtro, sender);

        }
        private void Incid_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref incid_Filtro, sender);

        }
        private void TipoPza_Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObtValorString(ref tipoPza_Filtro, sender);

        }

        private void FIni_cambiada(object sender, SelectionChangedEventArgs e)
        {
            ObtValorFecha(ref fIni_Filtro, sender);

        }
        private void FFin_cambiada(object sender, SelectionChangedEventArgs e)
        {
            ObtValorFecha(ref fFin_Filtro, sender);

        }

        private void EnIFS_Filtro_Checked(object sender, RoutedEventArgs e)
        {
            //EnIFS_Filtro = (sender as CheckBox).IsChecked.ToString();
            enIFS_Filtro = (sender as CheckBox).IsChecked.ToString();
        }

        private void ObtValorString(ref string campo,object sender)
        {
            campo = (sender as TextBox).Text;
         }
        private void ObtValorcb(ref string campo, object sender)
        {
            campo = (sender as ComboBox).SelectedValue.ToString();
 
        }
        private void ObtValorFecha(ref string campo, object sender)
        {
            campo = (sender as DatePicker).SelectedDate.ToString();
            
        }


    }
}

