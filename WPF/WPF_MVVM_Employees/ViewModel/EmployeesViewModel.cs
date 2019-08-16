using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF_MVVM_Employees.Data;

namespace WPF_MVVM_Employees.ViewModel
{
    class EmployeesViewModel : DependencyObject
    {


        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(EmployeesViewModel), new PropertyMetadata(string.Empty, FiterText_Changed));

        private static void FiterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmployeesViewModel;

            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.FilterEmployee;
            }
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(collectionViewProperty); }
            set { SetValue(collectionViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for collectionView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty collectionViewProperty =
            DependencyProperty.Register("collectionView", typeof(ICollectionView), typeof(EmployeesViewModel), new PropertyMetadata(null));

        public EmployeesViewModel()
        {
            Items = CollectionViewSource.GetDefaultView(Employee.GetEmployees());
            DepItems = CollectionViewSource.GetDefaultView(Department.GetDepartments());
            isSellectByDep = false;
            Items.Filter = FilterEmployee;
        }

        private bool FilterEmployee(object obj)
        {
            bool result = true;
            Employee current = obj as Employee;

            if (!string.IsNullOrWhiteSpace(FilterText) &&  current != null && !(current.Name.Contains(FilterText) || current.Surname.Contains(FilterText)))
            {
                result = false;
            }
            if (isSellectByDep == true && current.DepID != SellectDep?.ID)
            {
                result = false;
            }
                return result;
        }

        private bool isSellectByDep;

        public Department SellectDep
        {
            get { return (Department)GetValue(SellectDepProperty); }
            set { SetValue(SellectDepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SellectDep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SellectDepProperty =
            DependencyProperty.Register("SellectDep", typeof(Department), typeof(EmployeesViewModel), new PropertyMetadata(null, SellectedDepChanged));

        private static void SellectedDepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EmployeesViewModel;

            if (current != null)
            {
                current.isSellectByDep = true;
                current.Items.Filter = null;
                current.Items.Filter = current.FilterEmployee;
            }
        }

        public ICollectionView DepItems
        {
            get { return (ICollectionView)GetValue(DepItemsProperty); }
            set { SetValue(DepItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DepItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepItemsProperty =
            DependencyProperty.Register("DepItems", typeof(ICollectionView), typeof(EmployeesViewModel), new PropertyMetadata(null));

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

        public ICommand CleanSellectDep
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    SellectDep = null;
                    isSellectByDep = false;
                    Items.Filter = null;
                    Items.Filter = FilterEmployee;
                },
                (obj) => isSellectByDep == true);
            }
        }



    }
}
