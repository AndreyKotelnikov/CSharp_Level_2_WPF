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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Employees
{
    //    Курс C# 2 - урок 5 и 6 Практическое задание
    //    Выполнил Андрей Котельников
    //    Задание:
    //    1.	Создать сущности Employee и Department и заполнить списки сущностей начальными данными.
    //    2.	Для списка сотрудников и списка департаментов предусмотреть визуализацию(отображение). Это можно сделать, например, с использованием ComboBox или ListView.
    //    3.	Предусмотреть редактирование сотрудников и департаментов.Должна быть возможность изменить департамент у сотрудника.Список департаментов для выбора можно выводить в ComboBox, и все это можно выводить на дополнительной форме.
    //    4.	Предусмотреть возможность создания новых сотрудников и департаментов.Реализовать это либо на форме редактирования, либо сделать новую форму.
    //
    //    5.    Изменить WPF-приложение для ведения списка сотрудников компании (из урока 5), используя связывание данных, ListView, ObservableCollection и INotifyPropertyChanged.




    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Repository Repo { get; set; }
        
        /// <summary>
        /// Создаёт главное окно
        /// </summary>
        public MainWindow()
        {
            Repo = new Repository();
            Repo.FillDepartments();
            Repo.FillEmployees();

            InitializeComponent();

            Style = (Style)FindResource("MyWindowsStyle");
            btnSelectOff.Style = (Style)FindResource("MyButtonsStyle");

            Repo.ListDepartments.CollectionChanged += ListDepartments_CollectionChanged;
            gridEmployeesComboBox.ItemsSource = Repo.ListDepartments;
            
        }

        private void ListDepartments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Binding bind = new Binding();
            bind.Source = Repo.ListDepartments;
            bind.Mode = BindingMode.OneWay;
            bind.Converter = new DepartmentAddNullConverter();
            bind.ConverterParameter = "(Пусто)";
            selectionByDep.SetBinding(ItemsControl.ItemsSourceProperty, bind);


        }

        /// <summary>
        /// Обновляет данные на форме окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            gridEmployees.Items.Refresh();
        }

        /// <summary>
        /// Запускает дочернее окно для добавления нового сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_New_Emp(object sender, RoutedEventArgs e)
        {
            NewEmployeeWindow newEmployeeWindow = new NewEmployeeWindow(Repo.ListEmployees, Repo.ListDepartments);
            newEmployeeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newEmployeeWindow.Owner = this;
            newEmployeeWindow.ShowDialog();
            
        }

        /// <summary>
        /// Запускает дочернее окно для добавления нового департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_New_Dep(object sender, RoutedEventArgs e)
        {
            NewDepartmentWindow newDepartmentWindow = new NewDepartmentWindow(Repo.ListDepartments);
            newDepartmentWindow.Owner = this;
            newDepartmentWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newDepartmentWindow.ShowDialog();
        }


        private void SelectionByDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Department selectedDep = comboBox.SelectedItem as Department;
            
            gridEmployees.ItemsSource = Repo.ListEmployees.Where(emp => emp.Department == selectedDep);

            btnSelectOff.Visibility = Visibility.Visible;

        }

        private void Button_Click_Cancel_Selection(object sender, RoutedEventArgs e)
        {
            
            selectionByDep.SelectedValue = null;
            
            gridEmployees.ItemsSource = Repo.ListEmployees;

            btnSelectOff.Visibility = Visibility.Hidden;
        }
    }
}
