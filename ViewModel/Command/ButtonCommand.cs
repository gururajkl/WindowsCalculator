using System;
using System.Windows.Input;

namespace Calculator.ViewModel.Command
{
    public class ButtonCommand : ICommand
    {
        public CalculatorVM VM { get; set; }
        public ButtonCommand(CalculatorVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            string selectedOption = parameter as string;
            VM.CommandParameter(selectedOption);
        }
    }
}
