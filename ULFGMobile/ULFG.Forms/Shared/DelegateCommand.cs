using System;
using System.Windows.Input;

namespace ULFG.Forms.Shared
{
    /// <summary>
    /// Clase basada en <see cref="ICommand"/> para ser utilizada en las clases del modelo de las vistas
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Crea el comando
        /// </summary>
        /// <param name="execute">acción a ejecutar</param>
        /// <param name="canExecute">función para resolver si se puede ejecutar</param>
        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determina si el comando se puede ejecutar
        /// </summary>
        /// <param name="parameter">un parámetro</param>
        /// <returns>True si se puede ejecutar, Falso en caso contrario</returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute();
        }

        /// <summary>
        /// Indica si la función para indicar que el comando se puede ejecutar ha cambiado
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Ejecuta el comando
        /// </summary>
        /// <param name="parameter">un parámetro</param>
        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute();
        }

        /// <summary>
        /// Lanza un evento para indicar que el comando se puede ejecutar ha cambiado
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handle = CanExecuteChanged;
            if (handle != null)
                handle(this, new EventArgs());
        }
    }
}
