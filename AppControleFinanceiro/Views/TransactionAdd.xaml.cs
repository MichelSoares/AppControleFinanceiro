using System.Text;
using AppControleFinanceiro.Model;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _repository;
    public TransactionAdd(ITransactionRepository repository)
    {
        _repository = repository;
        InitializeComponent();
    }

    private void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
#if ANDROID
        if(Platform.CurrentActivity.CurrentFocus != null)
        {
            //Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
            Platform.CurrentActivity.DismissKeyboardShortcutsHelper();
        }
#endif
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (IsValidData().Result == false) return;
        SaveTransactionInDatabase();
        
        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);

        //var count = _repository.GetAll().Count;
        //MsgToast($"Mensagem! existem {count} registro(s) no banco!");
#if ANDROID
        if(Platform.CurrentActivity.CurrentFocus != null)
        {
            //Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
            //Platform.CurrentActivity.;
        }
#endif

    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = Convert.ToDouble(EntryValue.Text)
        };

        //var repository = Handler.MauiContext.Services.GetService<ITransactionRepository>();
        _repository.Add(transaction);
    }

    private async Task<bool> IsValidData()
    {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            valid = false;
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            valid = false;
        }
        double result;
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            sb.AppendLine("O campo 'Valor' é inválido!");
            valid |= false;
        }

        if (valid == false)
        {
            await MsgToast(sb.ToString());
            //LabelError.Text = sb.ToString();
        }

        return valid;
    }

    private static async Task MsgToast(string sb)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var toast = Toast.Make(sb.ToString(), ToastDuration.Long, 14);
        await toast.Show(cancellationTokenSource.Token);
    }

}