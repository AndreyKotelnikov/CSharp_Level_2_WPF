using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WPF_MVVM_Employees_DataTable.Models;
using WPF_MVVM_Employees_DataTable.Views;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{

    /// <summary>
    /// Класс ViewModel для привязки Views и взаимодействия с Models
    /// </summary>
    class EmpViewModel : DependencyObject
    {
        /// <summary>
        /// Привязывает источник получения данных к типу класса, в котором он реализован
        /// </summary>
        Dictionary<TypeOfDataSource, Type> dataSources;
        
        /// <summary>
        /// Привязывает название таблицы и интерфейс, через который обновляются её данные
        /// </summary>
        Dictionary<string, Type> tables;
        
        /// <summary>
        /// Привязывает название таблицы и метод проверки таблицы на наличие необходимых столбцов
        /// </summary>
        Dictionary<string, Action<DataView>> validationTables;
        
        /// <summary>
        /// Привязывает пользовательскую строку подключения к источнику данных. Инициализируется и добавляются значения по мере необходимости.
        /// </summary>
        Dictionary<TypeOfDataSource, string> userConnectionStrings;

        /// <summary>
        /// Коструктор класса, в котором задаются привязки для работы с таблицами и источниками данных
        /// </summary>
        public EmpViewModel()
        {
            dataSources = new Dictionary<TypeOfDataSource, Type>();
            dataSources.Add(TypeOfDataSource.StaticData, typeof(EmployeesStaticData));
            dataSources.Add(TypeOfDataSource.SqlDataBase, typeof(EmployeesDataBase));
            dataSources.Add(TypeOfDataSource.WebServiceAPI, typeof(EmployeesWebAPI));

            tables = new Dictionary<string, Type>();
            tables.Add("DataViewEmp", typeof(IEmployeesData));
            tables.Add("DataViewDep", typeof(IDepartmentsData));

            validationTables = new Dictionary<string, Action<DataView>>();
            validationTables.Add("DataViewEmp", DataBaseValidation.EmpValidete);
            validationTables.Add("DataViewDep", DataBaseValidation.DepValidete);

            GetDataView();
        }

        #region Свойства зависимости для привязки с Views


        /// <summary>
        /// Таблица сотрудников
        /// </summary>
        public DataView DataViewEmp
            {
            get { return (DataView)GetValue(DataViewEmpProperty); }
            set { SetValue(DataViewEmpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataViewEmp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataViewEmpProperty =
            DependencyProperty.Register("DataViewEmp", typeof(DataView), typeof(EmpViewModel), new PropertyMetadata(null));




        /// <summary>
        /// Таблица департаментов
        /// </summary>
        public DataView DataViewDep
        {
            get { return (DataView)GetValue(DataViewDepProperty); }
            set { SetValue(DataViewDepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataViewDep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataViewDepProperty =
            DependencyProperty.Register("DataViewDep", typeof(DataView), typeof(EmpViewModel), new PropertyMetadata(null));




        /// <summary>
        /// Тип источника данных, из которого заполняются таблицы с данными
        /// </summary>
        public TypeOfDataSource TypeOfDataSource
        {
            get { return (TypeOfDataSource)GetValue(TypeOfDataSourceProperty); }
            set { SetValue(TypeOfDataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfDataSourceProperty =
            DependencyProperty.Register("TypeOfDataSource", typeof(TypeOfDataSource), typeof(EmpViewModel), new PropertyMetadata(TypeOfDataSource.StaticData, Changed_TypeOfDataSource));

        /// <summary>
        /// При смене типа источника данных проверяется наличие строки подключения по умолчанию и запускается получение данных 
        /// </summary>
        /// <param name="d">Объект, к которому привязано данное свойство зависимости</param>
        /// <param name="e">Аргумент со старым и новым значением свойства</param>
        private static void Changed_TypeOfDataSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmpViewModel;
            if(current != null)
            {
                current.ConnectionString = current.userConnectionStrings?.FirstOrDefault(s => s.Key == current.TypeOfDataSource).Value ??
                ConfigurationManager.ConnectionStrings[current.TypeOfDataSource.ToString()]?.ConnectionString;

                if (current.GetDataView())
                {
                    current.SetUserConnectionString(current.ConnectionString);
                }
                else
                {
                    foreach (var item in current.tables)
                    {
                        current.GetType().GetProperty(item.Key).SetValue(current, null);
                    }
                    current.SetUserConnectionString(null);
                }
            }
        }




        /// <summary>
        /// Строка подключения для получения и обновления данных в таблицах
        /// </summary>
        public string ConnectionString
        {
            get { return (string)GetValue(ConnectionStringProperty); }
            set { SetValue(ConnectionStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectionString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionStringProperty =
            DependencyProperty.Register("ConnectionString", typeof(string), typeof(EmpViewModel), new PropertyMetadata(null));




        /// <summary>
        /// Тип обновления данных
        /// </summary>
        public TypeOfUpdateData TypeOfUpdateData
        {
            get { return (TypeOfUpdateData)GetValue(TypeOfUpdateDataProperty); }
            set { SetValue(TypeOfUpdateDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfUpdateData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfUpdateDataProperty =
            DependencyProperty.Register("TypeOfUpdateData", typeof(TypeOfUpdateData), typeof(EmpViewModel), new PropertyMetadata(TypeOfUpdateData.automatic, TypeOfUpdateDataPropertyChanged));

        /// <summary>
        /// При изменении типа обновления данных: запускает обновление данных
        /// </summary>
        /// <param name="d">Объект, к которому привязано данное свойство зависимости</param>
        /// <param name="e">Аргумент со старым и новым значением свойства</param>
        private static void TypeOfUpdateDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmpViewModel;

            current?.Update();
        }



        

        /// <summary>
        /// Текст для фильтрации таблицы сотрудников по ФИО
        /// </summary>
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(EmpViewModel), new PropertyMetadata(string.Empty, FiterText_Changed));

        /// <summary>
        /// При изменении текста для фильтрации: обновляет фильтр у таблицы сотрудников
        /// </summary>
        /// <param name="d">Объект, к которому привязано данное свойство зависимости</param>
        /// <param name="e">Аргумент со старым и новым значением свойства</param>
        private static void FiterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmpViewModel;

            if (current != null && current.DataViewEmp != null)
            {
                try
                {
                    current.DataViewEmp.RowFilter = current.GetRowFilter();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка при вводе текста для поиска:");
                }
                
            }
        }




        /// <summary>
        /// ID выбранного департамента. Если департамент не выбран, то значение null. 
        /// </summary>
        public int? SellectDepID
        {
            get { return (int?)GetValue(SellectDepIDProperty); }
            set { SetValue(SellectDepIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellectDepID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellectDepIDProperty =
            DependencyProperty.Register("SellectDepID", typeof(int?), typeof(EmpViewModel), new PropertyMetadata(null, SellectedDepChanged));

        /// <summary>
        /// При изменении выбранного департамента: обновляет фильтр у таблицы сотрудников 
        /// </summary>
        /// <param name="d">Объект, к которому привязано данное свойство зависимости</param>
        /// <param name="e">Аргумент со старым и новым значением свойства</param>
        private static void SellectedDepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmpViewModel;

            if (current != null)
            {
                current.DataViewEmp.RowFilter = current.GetRowFilter();
            }
        }




        /// <summary>
        /// Первый сотрудник из выбранных сотрудников. Если сотрудник не выбран, то значение null.
        /// </summary>
        public int? SellectedEmpID
        {
            get { return (int?)GetValue(SellectedItemProperty); }
            set { SetValue(SellectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellectedEmpID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellectedItemProperty =
            DependencyProperty.Register("SellectedEmpID", typeof(int?), typeof(EmpViewModel), new PropertyMetadata(null, SellectedEmpID_Changed));

        /// <summary>
        /// При изменении выбранного сотрудника: запускает обновление данных
        /// </summary>
        /// <param name="d">Объект, к которому привязано данное свойство зависимости</param>
        /// <param name="e">Аргумент со старым и новым значением свойства</param>
        private static void SellectedEmpID_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EmpViewModel current = d as EmpViewModel;
            current?.Update();

        }




        /// <summary>
        /// Новая таблица для создания нового сотрудника
        /// </summary>
        public DataView EmpNewRows
        {
            get { return (DataView)GetValue(EmpNewRowsProperty); }
            set { SetValue(EmpNewRowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmpNewRow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmpNewRowsProperty =
            DependencyProperty.Register("EmpNewRows", typeof(DataView), typeof(EmpViewModel), new PropertyMetadata(null));




        /// <summary>
        /// Новая таблица для создания нового департамента
        /// </summary>
        public DataView DepNewRows
        {
            get { return (DataView)GetValue(DepNewRowsProperty); }
            set { SetValue(DepNewRowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DepNewRow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepNewRowsProperty =
            DependencyProperty.Register("DepNewRows", typeof(DataView), typeof(EmpViewModel), new PropertyMetadata(null));


        #endregion

        #region Команды для привязки с кнопками из Views

        

        /// <summary>
        /// Очищяет текст для фильтрации
        /// </summary>
        public ICommand CleanFilterText
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    FilterText = string.Empty;
                },
                (obj) => FilterText != string.Empty);
            }
        }

        /// <summary>
        /// Очищает выбор по департаменту
        /// </summary>
        public ICommand CleanSellectDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    SellectDepID = null;
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
                    EmpNewRows = GetNewRows(DataViewEmp);
                    if (obj != null && !IsWindowOpen(obj))
                    {
                        obj = CreatNewObject(obj);
                    }
                    
                    (obj as Window)?.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Добавляет нового сотрудника в таблицу сотрудников и закрывает окно для создания нового сотрудника
        /// </summary>
        public ICommand AddNewEmpToList
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    DataViewEmp?.Table.Merge(EmpNewRows.Table);
                    Update();
                    (obj as Window)?.Close();
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
                DepNewRows = GetNewRows(DataViewDep);
                if (obj != null && !IsWindowOpen(obj))
                {
                    obj = CreatNewObject(obj);
                }
                (obj as Window)?.ShowDialog();
                });
            }
        }

        /// <summary>
        /// Добавляет новый департамент в таблицу департаментов и закрывает окно для создания нового департамента
        /// </summary>
        public ICommand AddNewDepToList
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    DataViewDep?.Table.Merge(DepNewRows.Table);
                    Update();
                    (obj as Window)?.Close();
                });
            }
        }

        /// <summary>
        /// Открывает новое окно для редактирования таблицы департаментов
        /// </summary>
        public ICommand EditDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (obj != null && !IsWindowOpen(obj))
                    {
                        obj = CreatNewObject(obj);
                    }
                    (obj as Window)?.ShowDialog();
                },
                (obj) => DataViewDep?.Count > 0);
            }
        }

       

        /// <summary>
        /// Удаляет выбранный департамент из таблицы департаментов (ID департамента == SellectDepID)
        /// </summary>
        public ICommand DeleteDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    
                    DataViewDep.Table.Rows.Find(SellectDepID).Delete();
                    CleanSellectDep.Execute(null);
                    Update();
                },
                (obj) =>
                {
                    bool result = true;

                    if (SellectDepID != null)
                    {
                        foreach (var item in DataViewEmp.Table.Rows)
                        {
                            if ((item as DataRow).RowState != DataRowState.Deleted && (item as DataRow).Field<int?>("DepID") == SellectDepID)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                    
                    return result;
                });
            }
        }

        /// <summary>
        /// Удаляет выбранного сотрудника из таблицы сотрудников. Если переданный в параметре объект поддерживает множественное выделение, то удаляются все выделенные строки.
        /// </summary>
        public ICommand DeleteSellectRowsEmp
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    IList<object> sellectList = (obj as MultiSelector)?.SelectedItems as IList<object>;
                    if (sellectList == null)
                    {
                        DataViewEmp.Table.Rows.Find(SellectedEmpID).Delete();
                    }
                    else
                    {
                        for (int i = sellectList.Count - 1 ; i >= 0; i--)
                        {
                            (sellectList[i] as DataRowView).Row.Delete();
                        }
                    }

                    Update();

                }, (obj) => SellectedEmpID != null);
            }
        }

        /// <summary>
        /// Удаляет всех сотрудников из таблицы сотрудников, которые успешно прошли условия фильтрации для отображения сотрудников
        /// </summary>
        public ICommand DeleteDisplayedRowsEmp
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    DataTable dataTable = DataViewEmp.ToTable();
                    
                    foreach (var item in dataTable.Rows)
                    {
                        DataViewEmp.Table.Rows.Find((item as DataRow).Field<int>("ID")).Delete();
                    }

                    Update();

                }, (obj) => DataViewEmp?.Count > 0);
            }
        }

        /// <summary>
        /// Запускает обновление данных
        /// </summary>
        public ICommand UpdateDataBase
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Update();
                }, (obj) => (TypeOfUpdateData == TypeOfUpdateData.manual) && HaveChanges());
            }
        }

        /// <summary>
        /// Запускает отмену изменений в таблицах
        /// </summary>
        public ICommand RejectChangesCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    RejectChanges();
                    HaveChanges();
                }, (obj) => (TypeOfUpdateData == TypeOfUpdateData.manual) && HaveChanges());
            }
        }

        /// <summary>
        /// Запускает получение данных из источника данных. И при успешном выполнении сохраняет текущую строку подключения в userConnectionStrings
        /// </summary>
        public ICommand ConnectToData
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    if (GetDataView())
                    {
                        SetUserConnectionString(ConnectionString);
                    }

                }, (obj) => userConnectionStrings != null && userConnectionStrings.ContainsKey(TypeOfDataSource) && userConnectionStrings[TypeOfDataSource] != ConnectionString);
            }
        }

        #endregion

        #region Инкапсулированные методы для обеспечения взаимодействия Views и Models



        /// <summary>
        /// Заполняет данные в таблицах
        /// </summary>
        /// <returns>Возвращает true, если получение данных прошло успешно. Возвращает false, если были ошибки</returns>
        private bool GetDataView() => FillDataView();

        /// <summary>
        /// Обновляет данные в таблицах
        /// </summary>
        /// <param name="propertyName">Имя свойства, которое вызвало этот метод</param>
        /// <returns>Возвращает true, если обновление данных прошло успешно. Возвращает false, если были ошибки</returns>
        private bool Update([CallerMemberName] string propertyName = null)
        {
            if (TypeOfUpdateData == TypeOfUpdateData.automatic || (TypeOfUpdateData == TypeOfUpdateData.manual && propertyName == "UpdateDataBase"))
            {
                return FillDataView();
            }
            return true;
        }

        /// <summary>
        /// Запускает указанный метод для каждой таблицы 
        /// </summary>
        /// <param name="methodName">Имя вызвавшего этот метод метода, который нужно запустить для каждой таблицы</param>
        /// <returns>Возвращает true, если выполнение метода прошло успешно. Возвращает false, если были ошибки</returns>
        private bool FillDataView([CallerMemberName] string methodName = null)
        {
            DataView dv;

            foreach (var item in tables)
            {
                try
                {
                    dv = (DataView)item.Value.GetMethod(methodName).Invoke(GetInstance(dataSources[TypeOfDataSource]),
                         methodName == "GetDataView" ? new object[] { ConnectionString } : null);
                }
                catch (SqlException ex)
                {
                    ExceptionHandler(ex, methodName, item.Key, "Ошибка базы данных");
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionHandler(ex, methodName, item.Key);
                    return false;
                }
                if (dv != null)
                {
                    if (methodName == "GetDataView")
                    {
                        validationTables[item.Key].Invoke(dv);
                        CreatePrimaryKey(dv);
                    }
                    GetType().GetProperty(item.Key).SetValue(this, dv);
                }
            }
            return true;
        }

        /// <summary>
        /// Показывает пользователю сообщение об ошибке и его детали
        /// </summary>
        /// <param name="ex">Объект с информацией об ошибке</param>
        /// <param name="methodName">Имя метода, при выполнении которого появилась ошибка</param>
        /// <param name="tableName">Имя таблицы, при обновлении данных в которой появилась ошибка</param>
        /// <param name="caption">Заголовок окна, в котором будут отображаться детали ошибки</param>
        private void ExceptionHandler(Exception ex, string methodName, string tableName, string caption = null)
        {
            Exception innerEx = GetInnerException(ex);
            MessageBox.Show(innerEx.Message, caption ?? $"Class:{dataSources[TypeOfDataSource].Name.ToString()}, Method:{methodName}, Table:{tableName}");

            if (TypeOfUpdateData == TypeOfUpdateData.automatic) { RejectChanges(GetType().GetProperty(tableName).GetValue(this) as DataView); }
        }

        /// <summary>
        /// Получает самое глубоко вложенное исключение 
        /// </summary>
        /// <param name="ex">Исключение, которое имеет вложенное исключение</param>
        /// <returns>Возвращает самое глубоко вложенное исключение</returns>
        private Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException == null) { return ex; }
            return GetInnerException(ex.InnerException);
        }

        /// <summary>
        /// Получает ссылку на объект указанного типа
        /// </summary>
        /// <param name="type">Тип, для которого требуется получить ссылку на экзепляр</param>
        /// <returns>Ссылка на объект указанного типа</returns>
        private object GetInstance(Type type)
        {
            return type.GetProperty("Instance").GetValue(type);
        }

        /// <summary>
        /// Задаёт первичный ключ по ID в указанной таблице
        /// </summary>
        /// <param name="dv">Таблица для задания первичного ключа</param>
        private void CreatePrimaryKey(DataView dv)
        {
            DataTable dt = dv.Table;
            DataColumn[] arrEmp = new DataColumn[dt.Columns.Count];
            dt.Columns.CopyTo(arrEmp, 0);
            DataColumn[] keysEmp = { arrEmp.First(c => c.ColumnName == "ID") };
            dt.PrimaryKey = keysEmp;
        }

        /// <summary>
        /// Отменяет изменения в указанной таблице. Если таблица не указана, то отменяются изменения во всех таблицах
        /// </summary>
        /// <param name="dv">Таблица, в которой нужно отменить изменения</param>
        private void RejectChanges(DataView dv = null)
        {
            if (dv != null)
            {
                dv.Table.RejectChanges();
                return;
            }

            foreach (var item in tables)
            {
                (GetType().GetProperty(item.Key).GetValue(this) as DataView)?.Table.RejectChanges();
            }
        }

        /// <summary>
        /// Получает строку со скриптом фильтрации для представления таблицы
        /// </summary>
        /// <returns>Строка со скриптом фильтрации</returns>
        private string GetRowFilter()
        {

            string strName_Surname = string.Format("(Name LIKE '%{0}%' OR Surname LIKE '%{0}%')", FilterText);
            string result = strName_Surname;
            string strDepSellect;
            if (SellectDepID != null)
            {
                strDepSellect = string.Format(" AND DepID = {0}", SellectDepID);
                result += strDepSellect;
            }
            return result;
        }

        /// <summary>
        /// Проверяет наличие изменений в таблицах
        /// </summary>
        /// <returns>True, если в таблицах есть изменения. False, если изменений нет</returns>
        private bool HaveChanges()
        {
            foreach (var item in tables)
            {
                if ((GetType().GetProperty(item.Key).GetValue(this) as DataView)?.Table.GetChanges() != null)
                {
                    return true;
                };
            }
            return false;
        }

        /// <summary>
        /// Создаёт копию указанной таблицы с одной пустой строчкой, у которой заполняется ID
        /// </summary>
        /// <param name="dataView">Таблица, с которой будет делаться копия таблицы</param>
        /// <returns>Таблица с пустой строкой и новым ID</returns>
        private DataView GetNewRows(DataView dataView)
        {
            if (dataView == null) { return null; }
            DataTable dt = dataView.Table.Clone();
            DataRow row = dt.NewRow();
            object[] itemArr = row.ItemArray;
            int ? maxID = dataView.Table.Rows.Cast<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Max(r => r.Field<int?>("ID")) ?? -1;
            itemArr[0] = maxID + 1;
            row.ItemArray = itemArr;
            dt.Rows.Add(row);
            return dt.DefaultView;
        }

        /// <summary>
        /// Создаёт новый объект с типом указанного объекта
        /// </summary>
        /// <param name="obj">Объект, с типом которого нужно создать новый объект</param>
        /// <returns>Новый объект с типом указанного объекта</returns>
        private object CreatNewObject(object obj)
        {
            var result = Activator.CreateInstanceFrom(obj.GetType().Assembly.GetName().CodeBase, obj.GetType().FullName);
            return result.Unwrap();
        }

        /// <summary>
        /// Проверяет открыто ли хотя бы одно окно с типом указанного объекта
        /// </summary>
        /// <param name="obj">Окно, по которому нужно проверить наличие открытого окна этого же типа</param>
        /// <returns>True, если есть открытое окно с типом указанного объета. False, если открытое окно данного типа не найдено</returns>
        private bool IsWindowOpen(object obj)
        {
            bool result = false;
            foreach (var item in App.Current.Windows)
            {
                if (item.GetType() == obj.GetType())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Добавляет указанную строку в userConnectionStrings по ключу = текущее значение TypeOfDataSource
        /// </summary>
        /// <param name="connectString">Строка, которую нужно добавить в userConnectionStrings</param>
        private void SetUserConnectionString(string connectString = null)
        {
            if (userConnectionStrings == null) { userConnectionStrings = new Dictionary<TypeOfDataSource, string>(); }

            if (userConnectionStrings.ContainsKey(TypeOfDataSource))
            {
                userConnectionStrings[TypeOfDataSource] = connectString;
            }
            else
            {
                userConnectionStrings.Add(TypeOfDataSource, connectString);
            }
        }

        #endregion
    }


}
