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
    //    Курс C# 2 - урок 5 Практическое задание
    //    Выполнил Андрей Котельников
    //    Задание:
    //    1.	Создать сущности Employee и Department и заполнить списки сущностей начальными данными.
    //    2.	Для списка сотрудников и списка департаментов предусмотреть визуализацию(отображение). Это можно сделать, например, с использованием ComboBox или ListView.
    //    3.	Предусмотреть редактирование сотрудников и департаментов.Должна быть возможность изменить департамент у сотрудника.Список департаментов для выбора можно выводить в ComboBox, и все это можно выводить на дополнительной форме.
    //    4.	Предусмотреть возможность создания новых сотрудников и департаментов.Реализовать это либо на форме редактирования, либо сделать новую форму.




    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Repository repo = new Repository();

        /// <summary>
        /// Создаёт главное окно
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            repo.FillDepartments();
            repo.FillEmployees();

            gridEmployees.ItemsSource = repo.ListEmployees;
            gridEmployeesComboBox.ItemsSource = repo.ListDepartments;
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
            NewEmployeeWindow newEmployeeWindow = new NewEmployeeWindow(repo.ListEmployees, repo.ListDepartments);
            newEmployeeWindow.Owner = this;
            newEmployeeWindow.ShowDialog();
            gridEmployees.Items.Refresh();
        }

        /// <summary>
        /// Запускает дочернее окно для добавления нового департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_New_Dep(object sender, RoutedEventArgs e)
        {
            NewDepartmentWindow newDepartmentWindow = new NewDepartmentWindow(repo.ListDepartments);
            newDepartmentWindow.Owner = this;
            newDepartmentWindow.ShowDialog();
            gridEmployees.Items.Refresh();
        }
    }
}
