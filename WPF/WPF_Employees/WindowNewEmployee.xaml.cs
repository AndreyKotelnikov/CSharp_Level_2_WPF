using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Employees
{
    /// <summary>
    /// Логика взаимодействия для WindowNewEmployee.xaml
    /// </summary>
    public partial class NewEmployeeWindow : Window
    {
        /// <summary>
        /// Список текущих сотрудников
        /// </summary>
        ObservableCollection<Employee> listEmp;
        /// <summary>
        /// Список новых сотрудников
        /// </summary>
        ObservableCollection<Employee> newListEmp = new ObservableCollection<Employee>();

        /// <summary>
        /// Создаёт дочернее окно для добавления нового сотрудника
        /// </summary>
        /// <param name="listEmp">Список текущих сотрудников</param>
        /// <param name="listDep">Список департаментов</param>
        public NewEmployeeWindow(ObservableCollection<Employee> listEmp, ObservableCollection<Department> listDep)
        {
            InitializeComponent();

            Style = (Style)FindResource("MyWindowsStyle");

            Employee newEmp = new Employee();
            newListEmp.Add(newEmp);
            gridNewEmployee.ItemsSource = newListEmp;
            gridNewEmployeesComboBox.ItemsSource = listDep;
            this.listEmp = listEmp;

        }
        /// <summary>
        /// Добавляет в список текущих сотрудников новых сотрудников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            foreach (var emp in newListEmp)
            {
                listEmp.Add(emp);
            }
            this.Close();
        }
    }
}
