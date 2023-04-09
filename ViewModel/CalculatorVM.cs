using System;
using System.Collections.Generic;
using System.ComponentModel;
using Calculator.ViewModel.Command;
using Calculator.Model;

namespace Calculator.ViewModel
{
    public class CalculatorVM : INotifyPropertyChanged
    {
        string valueLeft = "";
        string selectedOperatorForSwitch = "";
        List<string> operators = new List<string> { "+", "-", "/", "%", "×" };

        public CalculatorVM()
        {
            ButtonCommand = new ButtonCommand(this);
            CalculatorOperands = new OperandAndOperator();
            SelectedOperator = new OperandAndOperator();
        }

        private OperandAndOperator _calculatorOperands;
        
        public OperandAndOperator CalculatorOperands
        {
            get { return _calculatorOperands; }
            set
            {
                _calculatorOperands = value;
            }
        }

        private OperandAndOperator _selectedOperator;
        public OperandAndOperator SelectedOperator
        {
            get { return _selectedOperator; }
            set { _selectedOperator = value; }
        }

        public ButtonCommand ButtonCommand { get; set; }

        public void CommandParameter(string parameter)
        {
            try
            {
                if (operators.Contains(parameter))
                {
                    if (!string.IsNullOrEmpty(CalculatorOperands.Operands))
                    {
                        valueLeft = CalculatorOperands.Operands;
                        SelectedOperator.Operator = string.Format("{0} {1}", valueLeft, parameter);
                        selectedOperatorForSwitch = parameter;
                        CalculatorOperands.Operands = "";
                        OnPropertyChanged("CalculatorOperands");
                        OnPropertyChanged("SelectedOperator");
                    }
                }
                else if (parameter == "=")
                {
                    #region Operations
                    if (SelectedOperator.Operator.Contains("="))
                    {
                        string[] newValue = SelectedOperator.Operator.Split(' ');
                        valueLeft = newValue[0];
                    }
                    switch (selectedOperatorForSwitch)
                    {
                        case "+":
                            Result(parameter, selectedOperatorForSwitch);
                            CalculatorOperands.Operands = Convert.ToString(Convert.ToDouble(valueLeft) + Convert.ToDouble(CalculatorOperands.Operands));
                            break;
                        case "-":
                            Result(parameter, selectedOperatorForSwitch);
                            CalculatorOperands.Operands = Convert.ToString(Convert.ToDouble(valueLeft) - Convert.ToDouble(CalculatorOperands.Operands));
                            break;
                        case "×":
                            Result(parameter, selectedOperatorForSwitch);
                            CalculatorOperands.Operands = Convert.ToString(Convert.ToDouble(valueLeft) * Convert.ToDouble(CalculatorOperands.Operands));
                            break;
                        case "/":
                            Result(parameter, selectedOperatorForSwitch);
                            CalculatorOperands.Operands = (Convert.ToDouble(valueLeft) / Convert.ToDouble(CalculatorOperands.Operands)).ToString("N4");
                            break;
                        case "%":
                            Result(parameter, selectedOperatorForSwitch);
                            CalculatorOperands.Operands = Convert.ToString(Convert.ToDouble(valueLeft) % Convert.ToDouble(CalculatorOperands.Operands));
                            break;
                    }
                    #endregion
                    OnPropertyChanged("CalculatorOperands");
                }
                else if (parameter == "AC")
                {
                    CalculatorOperands.Operands = "";
                    SelectedOperator.Operator = "";
                    OnPropertyChanged("CalculatorOperands");
                    OnPropertyChanged("SelectedOperator");
                }
                else if (parameter == "√")
                {
                    SelectedOperator.Operator = string.Format("√({0})", CalculatorOperands.Operands);
                    double result = Math.Sqrt(Convert.ToDouble(CalculatorOperands.Operands));
                    CalculatorOperands.Operands = result.ToString();
                    OnPropertyChanged("CalculatorOperands");
                    OnPropertyChanged("SelectedOperator");
                }
                else if (parameter == ".")
                {
                    if (!CalculatorOperands.Operands.Contains("."))
                    {
                        CalculatorOperands.Operands += ".";
                        OnPropertyChanged("CalculatorOperands");
                    }
                }
                else if (parameter == "C")
                {
                    if (CalculatorOperands.Operands != null)
                    {
                        int length = CalculatorOperands.Operands.Length;
                        if (length > 0)
                        {
                            string updatedValue = CalculatorOperands.Operands.Remove(length - 1);
                            CalculatorOperands.Operands = updatedValue;
                            OnPropertyChanged("CalculatorOperands");
                        }
                    }
                }
                else if (parameter == "power")
                {
                    if (!string.IsNullOrEmpty(CalculatorOperands.Operands))
                    {
                        double result = Convert.ToDouble(CalculatorOperands.Operands) * Convert.ToDouble(CalculatorOperands.Operands);
                        SelectedOperator.Operator = string.Format("{0}²", CalculatorOperands.Operands);
                        CalculatorOperands.Operands = result.ToString();
                        OnPropertyChanged("CalculatorOperands");
                        OnPropertyChanged("SelectedOperator");
                    }
                }
                else
                {
                    CalculatorOperands.Operands += parameter;
                    OnPropertyChanged("CalculatorOperands");
                }
            }
            catch (Exception)
            {
                CalculatorOperands.Operands = "Error!";
                OnPropertyChanged("CalculatorOperands");
            }
        }

        private void Result(string parameter, string sign)
        {
            if (!SelectedOperator.Operator.Contains("="))
            {
                SelectedOperator.Operator += string.Format(" {0} {1}", CalculatorOperands.Operands, parameter);
                OnPropertyChanged("SelectedOperator");
            }
            else
            {
                string[] newValue = SelectedOperator.Operator.Split(' ');
                SelectedOperator.Operator = string.Format("{0} {1} {2} =", newValue[0], sign, CalculatorOperands.Operands);
                OnPropertyChanged("SelectedOperator");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var helper = PropertyChanged;
            if (helper != null)
            {
                helper(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}