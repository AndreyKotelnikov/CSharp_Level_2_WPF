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
    /// Логика взаимодействия для NewDepartmentWindow.xaml
    /// </summary>
    public partial class NewDepartmentWindow : Window
    {
        /// <summary>
        /// Список текущих департаментов
        /// </summary>
        ObservableCollection<Department> departments;
        /// <summary>
        /// Список новых департаментов
        /// </summary>
        ObservableCollection<Department> listNewDep = new ObservableCollection<Department>();
        /// <summary>
        /// Создаёт дочернее окно для добавления нового департамента
        /// </summary>
        /// <param name="dep">Список текущих департаментов</param>
        public NewDepartmentWindow(ObservableCollection<Department> dep)
        {
            InitializeComponent();
            
            Style = (Style)FindResource("MyWindowsStyle");

            departments = dep;
            Department newDep = new Department(string.Empty);
            
            listNewDep.Add(newDep);
            gridNewDep.ItemsSource = listNewDep; 
        }
        /// <summary>
        /// Добавляет новые департаменты в список текущих департаментов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            foreach (var dep in listNewDep)
            {
                departments.Add(dep);
            }
            Close();
        }
    }
}
