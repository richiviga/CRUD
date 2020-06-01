using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using System.Data.Linq;

namespace CRUDDataGridWPF
{
    public partial class Window1 : Window
    {
        #region Constructor
        public Window1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
        } 
        #endregion

        #region Read Operation
        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            EmployeeDBDataContext context = new EmployeeDBDataContext();
            var result = from emp in context.Employees
                         select emp;
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
                EmployeeDBDataContext context = new EmployeeDBDataContext();
                Employee emp = e.Row.DataContext as Employee;

                var matchedData = (from em in context.GetTable<Employee>()
                                   where em.ID == emp.ID
                                   select em).SingleOrDefault();

                if (matchedData == null)
                {
                    Table<Employee> empTable = context.GetTable<Employee>();

                    Employee employee = new Employee();
                    employee.FirstName = emp.FirstName;
                    employee.LastName = emp.LastName;
                    employee.EmailID = emp.EmailID;
                    employee.Contact = emp.Contact;

                    empTable.InsertOnSubmit(employee);
                    empTable.Context.SubmitChanges();

                    txtStatus.Text = "Success: Data Inserted";
                }
                else
                {
                    matchedData.FirstName = emp.FirstName;
                    matchedData.LastName = emp.LastName;
                    matchedData.EmailID = emp.EmailID;
                    matchedData.Contact = emp.Contact;
                    context.SubmitChanges();

                    txtStatus.Text = "Success: Data Updated";
                }
            }
        } 
        #endregion

        #region Delete Operation
        private void dgData_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EmployeeDBDataContext context = new EmployeeDBDataContext();
            Employee employee = dgData.SelectedItem as Employee;
            
            if (employee != null)
            {
                var matchedCustomer = (from em in context.GetTable<Employee>()
                                   where em.ID == employee.ID
                                   select em).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("Are You Sure you want to Delete ?", 
                        "Confirm Delete !", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        context.Employees.DeleteOnSubmit(matchedCustomer);
                        context.SubmitChanges();
                        txtStatus.Text = "Success: Selected Data Deleted.";
                    }
                }
            }
        }
        #endregion
    }
}
