using Calculation_Parser;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace calculate_;
class MainViewModel : INotifyPropertyChanged, IDataErrorInfo
{
    private Calculator _calculator;

    public MainViewModel()
    {
        _calculator = new Calculator();

        InputTxt = string.Empty;

        Result = string.Empty;

        ButtonClicked = new RelayCommand<string>(OnDigitButton);

        ButtonCalculation = new RelayCommand<string>(Calculation);

        ButtonRemove = new RelayCommand(() =>
        {
            if (InputTxt.Length > 0)
            {
                InputTxt = InputTxt.Substring(0, InputTxt.Length - 1);
            }
        });

        CleanCommand = new RelayCommand(() =>
        {
            InputTxt = string.Empty;
            Result = string.Empty;
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(InputTxt));
        });
    }

    private void OnDigitButton(string digit)
    {
        if (digit != string.Empty)
        {
            InputTxt += digit;
            OnPropertyChanged(nameof(InputTxt));
        }
    }

    private void Calculation(string digit)
    {
        if (!string.IsNullOrEmpty(_inputTxt))
        {
            if (!_errors.Any())
            {
                Result = _calculator.StartParse(_inputTxt);
            }
        }
        OnPropertyChanged(nameof(InputTxt));
        OnPropertyChanged(nameof(Result));
    }

    private string _inputTxt;
    private string _result;


    public string InputTxt
    {
        get { return _inputTxt; }
        set
        {

            _inputTxt = value;
            OnPropertyChanged(nameof(InputTxt));

            if (string.IsNullOrEmpty(_inputTxt))
            {
                _errors.Clear();
            }
            /*if (string.IsNullOrWhiteSpace(value))
            {
                _errors[nameof(InputTxt)] = "пустая строка";
            }*/
            if (!string.IsNullOrEmpty(_inputTxt))
            {
                char lastChar = InputTxt[InputTxt.Length - 1];
                if ("/*-+".Contains(lastChar))
                {
                    _errors[nameof(InputTxt)] = "Введите полное арифметическое выражение";
                }
                else if ("/*-+".Contains(lastChar) && "/*-+".Contains(_inputTxt))
                {
                    InputTxt.Remove(lastChar);
                }
                else
                {
                    _errors[nameof(InputTxt)] = "";
                }
            }
            else
            {
                _errors[nameof(InputTxt)] = "";
            }



        }
    }

    public string Result
    {
        get { return _result; }
        set
        {
            if (_result != value)
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

    }


    public RelayCommand CleanCommand { get; }
    public RelayCommand ButtonRemove { get; }
    public RelayCommand<string> ButtonClicked { get; }

    public RelayCommand<string> ButtonCalculation { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private Dictionary<string, string> _errors = new Dictionary<string, string>();

    public string Error
    {
        get
        {
            return string.Join(Environment.NewLine, _errors);
        }
    }

    public string this[string columnName]
    {
        get
        {
            return _errors.TryGetValue(columnName, out var value) ? value : string.Empty;
        }
    }

}
