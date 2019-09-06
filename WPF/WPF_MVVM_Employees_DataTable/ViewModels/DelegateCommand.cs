using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_MVVM_Employees_DataTable.ViewModels
{
    /// <summary>
    /// Класс для создания команд
    /// </summary>
    class DelegateCommand : ICommand
    {
        /// <summary>
        /// Делегат с сигнатурой void (object)
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// Делегат с сигнатурой bool (object)
        /// </summary>
        private Func<object, bool> canExecute;

        /// <summary>
        /// Событие при изменении доступности команды для выполнения
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Клнструктор класса
        /// </summary>
        /// <param name="execute">Делегат с сигнатурой void (object)</param>
        /// <param name="canExecute">Делегат с сигнатурой bool (object)</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Проверяет условия, при которых доступна команда (разрешено выполнение метода Execute)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True -  команда доступна для выполнения</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Выполняет метод команды execute
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }

    }
}
