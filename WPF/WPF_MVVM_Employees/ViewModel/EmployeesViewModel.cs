using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF_MVVM_Employees.Data;
using WPF_MVVM_Employees.View;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Collections;
using Xamarin.Forms;

namespace WPF_MVVM_Employees.ViewModel
{
    class EmployeesViewModel : DependencyObject
    {
        /// <summary>
        /// Ссылка на форму окна для создания нового сотрудника
        /// </summary>
        private NewEmpWindow newEmpWindow;

        /// <summary>
        /// Ссылка на форму окна для создания нового департамента
        /// </summary>
        private NewDepWindow newDepWindow;

        /// <summary>
        /// Ссылка на форму окна для редактирования списка департаментов
        /// </summary>
        private EditDepWindow editDepWindow;

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public ICollectionView EmpItems
        {
            get { return (ICollectionView)GetValue(collectionViewProperty); }
            set { SetValue(collectionViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmpItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty collectionViewProperty =
            DependencyProperty.Register("collectionView", typeof(ICollectionView), typeof(EmployeesViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Список департаментов
        /// </summary>
        public ICollectionView DepItems
        {
            get { return (ICollectionView)GetValue(DepItemsProperty); }
            set { SetValue(DepItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DepItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepItemsProperty =
            DependencyProperty.Register("DepItems", typeof(ICollectionView), typeof(EmployeesViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Список департаментов с дополнительным пустым значением
        /// </summary>
        public DataTable DepItemsWithNull
        {
            get { return (DataTable)GetValue(DepItemsWithNullProperty); }
            set { SetValue(DepItemsWithNullProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DepItemsWithNull.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepItemsWithNullProperty =
            DependencyProperty.Register("DepItemsWithNull", typeof(DataTable), typeof(EmployeesViewModel), new PropertyMetadata(null));


        /// <summary>
        /// Текст для фильтрации списка сотрудников по ФИО
        /// </summary>
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(EmployeesViewModel), new PropertyMetadata(string.Empty, FiterText_Changed));

        /// <summary>
        /// Обновляет фильтр у списка сотрудников при изменении текста для фильтрации 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void FiterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmployeesViewModel;

            if (current != null)
            {
                current.EmpItems.Filter = null;
                current.EmpItems.Filter = current.FilterEmployee;
            }
        }

        /// <summary>
        /// ID выбранного департамента. Если департамент не выбран, то значение null. Если выбран пустой департамент, то значение -1.
        /// </summary>
        public int? SellectDepID
        {
            get { return (int?)GetValue(SellectDepProperty); }
            set { SetValue(SellectDepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellectDepID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellectDepProperty =
            DependencyProperty.Register("SellectDep", typeof(int?), typeof(EmployeesViewModel), new PropertyMetadata(null, SellectedDepChanged));

        /// <summary>
        /// Обновляет фильтр у списка сотрудников при изменении выбранного департамента
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void SellectedDepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmployeesViewModel;

            if (current != null)
            {
                current.EmpItems.Filter = null;
                current.EmpItems.Filter = current.FilterEmployee;
            }
        }

        /// <summary>
        /// Первый сотрудник из выбранных сотрудников
        /// </summary>
        public object SellectedItemEmp
        {
            get { return (object)GetValue(SellectedItemProperty); }
            set { SetValue(SellectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellectedItemEmp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellectedItemProperty =
            DependencyProperty.Register("SellectedItem", typeof(object), typeof(EmployeesViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Новая строка для создания нового сотрудника
        /// </summary>
        public ICollectionView EmpNewRow
        {
            get { return (ICollectionView)GetValue(EmpNewRowProperty); }
            set { SetValue(EmpNewRowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmpNewRow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmpNewRowProperty =
            DependencyProperty.Register("EmpNewRow", typeof(ICollectionView), typeof(EmployeesViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Новая строка для создания нового департамента
        /// </summary>
        public ICollectionView DepNewRow
        {
            get { return (ICollectionView)GetValue(DepNewRowProperty); }
            set { SetValue(DepNewRowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmpNewRow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepNewRowProperty =
            DependencyProperty.Register("DepNewRow", typeof(ICollectionView), typeof(EmployeesViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Конструктор класса без параметров
        /// </summary>
        public EmployeesViewModel()
        {
            EmpItems = CollectionViewSource.GetDefaultView(Employee.GetEmployees());
            EmpItems.Filter = FilterEmployee;
            DepItems = CollectionViewSource.GetDefaultView(Department.GetDepartments());
            CreateDepItemsWithNull();
            UpdateDepItemsWithNull();
        }

        /// <summary>
        /// Проверяет условия вхождения каждого сотрудника в отображаемый список сотрудников. 
        /// </summary>
        /// <param name="obj">Сотрудник</param>
        /// <returns>Значение true - проверка прошла успешно и этот сотрудник будет отображаться в списке. Значение false - сотрудник не будет отображаться в списке</returns>
        private bool FilterEmployee(object obj)
        {
            bool result = true;
            Employee current = obj as Employee;

            if (!string.IsNullOrWhiteSpace(FilterText) && current != null && !(current.Name.Contains(FilterText) || current.Surname.Contains(FilterText)))
            {
                result = false;
            }
            if (SellectDepID != null && SellectDepID != -1 && current.DepID != SellectDepID)
            {
                result = false;
            }
            if (SellectDepID == -1 && current.DepID != null)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Создаёт таблицу данных и колонки в ней
        /// </summary>
        private void CreateDepItemsWithNull()
        {
            DepItemsWithNull = new DataTable();

            DepItemsWithNull.Columns.Add(new DataColumn("ID", System.Type.GetType("System.Int32")));

            DepItemsWithNull.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
        }

        /// <summary>
        /// Перезаполняет таблицу данных DepItemsWithNull из списка департаментов DepItems и добавляет пустую строку
        /// </summary>
        private void UpdateDepItemsWithNull()
        {
            DepItemsWithNull.Clear();
            DataRow rowToAdd = DepItemsWithNull.NewRow();
            rowToAdd["ID"] = -1;
            rowToAdd["Name"] = string.Empty;
            DepItemsWithNull.Rows.Add(rowToAdd);
            foreach (var item in DepItems.SourceCollection as List<Department>)
            {
                rowToAdd = DepItemsWithNull.NewRow();
                rowToAdd["ID"] = item.ID;
                rowToAdd["Name"] = item.Name;
                DepItemsWithNull.Rows.Add(rowToAdd);
            }
        }

        /// <summary>
        /// Очищяет текст для фильтрации
        /// </summary>
        public ICommand CleanFilterText
        {
            get {
                return new DelegateCommand((obj) =>
                {
                    FilterText = string.Empty;
                },
                (obj) => FilterText != string.Empty);
            }
        }

        /// <summary>
        /// Ощищает выбор по департаменту
        /// </summary>
        public ICommand CleanSellectDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    SellectDepID = null;
                    EmpItems.Filter = null;
                    EmpItems.Filter = FilterEmployee;
                    UpdateDepItemsWithNull();
                },
                (obj) => SellectDepID != null);
            }
        }

        /// <summary>
        /// Открывает новое окно для создания нового сотрудника
        /// </summary>
        public ICommand CreatNewEmp
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    newEmpWindow = new NewEmpWindow();

                    Employee newEmp = new Employee();
                    List<Employee> list = new List<Employee>();
                    list.Add(newEmp);

                    EmpNewRow = CollectionViewSource.GetDefaultView(list);

                    newEmpWindow.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Добавляет нового сотрудника в список сотрудников и закрывает окно для создания нового сотрудника
        /// </summary>
        public ICommand AddNewEmpToList
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    List<Employee> list = (List<Employee>)(EmpItems as ListCollectionView).SourceCollection;
                    List<Employee> listNewEmp = (List<Employee>)(EmpNewRow as ListCollectionView).SourceCollection;
                    foreach (var item in listNewEmp)
                    {
                        list.Add(item as Employee);
                    }
                    newEmpWindow.Close();
                    EmpItems.Filter = null;
                    EmpItems.Filter = FilterEmployee;
                });
            }
        }

        /// <summary>
        /// Открывает новое окно для создания нового департамента
        /// </summary>
        public ICommand CreatNewDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    newDepWindow = new NewDepWindow();

                    Department newDep = new Department();
                    List<Department> list = new List<Department>();
                    list.Add(newDep);

                    DepNewRow = CollectionViewSource.GetDefaultView(list);

                    newDepWindow.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Добавляет новый департамент в список департаментов и закрывает окно для создания нового департамента
        /// </summary>
        public ICommand AddNewDepToList
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    List<Department> list = (List<Department>)(DepItems as ListCollectionView).SourceCollection;
                    List<Department> listNewDep = (List<Department>)(DepNewRow as ListCollectionView).SourceCollection;
                    foreach (var item in listNewDep)
                    {
                        list.Add(item as Department);
                    }

                    UpdateDepItemsWithNull();
                    DepItems.Refresh();

                    newDepWindow.Close();
                });
            }
        }

        /// <summary>
        /// Открывает новое окно для редактирования списка департаментов
        /// </summary>
        public ICommand EditDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    editDepWindow = new EditDepWindow();
                    editDepWindow.ShowDialog();
                    UpdateDepItemsWithNull();
                },
                (obj) => !DepItems.IsEmpty);
            }
        }

        /// <summary>
        /// Удаляет выбранный департамент из списка департаментов (ID департамента == SellectDepID)
        /// </summary>
        public ICommand DeleteDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    List<Department> list = (List<Department>)(DepItems as ListCollectionView).SourceCollection;

                    list.Remove(list.Find(o => o.ID == SellectDepID));

                    CleanSellectDep.Execute(obj);
                },
                (obj) =>
                {
                    bool result = false;
                    List<Employee> list = (List<Employee>)(EmpItems as ListCollectionView).SourceCollection;
                    if (SellectDepID != null && SellectDepID >= 0 && !list.Exists(o => o.DepID == SellectDepID)) { result = true; }

                    return result;
                });
            }
        }

        /// <summary>
        /// Удаляет выбранного сотрудника из списка сотрудников. Если переданный в параметре объект поддерживает множественное выделение, то удаляются все выделенные строки.
        /// </summary>
        public ICommand DeleteSellectRowsEmp
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    List<Employee> list = (List<Employee>)(EmpItems as ListCollectionView).SourceCollection;

                    IList<object> sellectList = (obj as MultiSelector)?.SelectedItems as IList<object>;
                    if (sellectList == null)
                    {
                        list.Remove(SellectedItemEmp as Employee);
                    }
                    else
                    {
                        foreach (var item in sellectList)
                        {
                            list.Remove(item as Employee);
                        }
                    }
                    EmpItems.Filter = null;
                    EmpItems.Filter = FilterEmployee;
                }, (obj) => SellectedItemEmp != null);
            }
        }

        /// <summary>
        /// Удаляет всех сотрудников из списка сотрудников, которые успешно прошли условия фильтрации объектов для отображения сотрудников
        /// </summary>
        public ICommand DeleteDisplayedRowsEmp
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    List<Employee> list = (List<Employee>)(EmpItems as ListCollectionView).SourceCollection;

                    for (int i = list.Count; i > 0; i--)
                    {
                        if (FilterEmployee(list[i - 1])) { list.Remove(list[i - 1] as Employee); }
                    }

                    EmpItems.Filter = null;
                    EmpItems.Filter = FilterEmployee;
                    
                }, (obj) => !(EmpItems as ListCollectionView).IsEmpty);
            }
        }
    }
}
