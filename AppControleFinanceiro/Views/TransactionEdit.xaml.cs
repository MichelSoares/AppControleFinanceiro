using AppControleFinanceiro.Model;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
	private Transaction _transaction;
    private ITransactionRepository _repository;

    public TransactionEdit(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    private void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {
		Navigation.PopModalAsync();
    }

	public void SetTransactionToEdit(Transaction transaction)
	{
		 _transaction = transaction;

		if(transaction.type == TransactionType.Income)
		
			RadioIncome.IsChecked = true;
		else
			RadioExpense.IsChecked = true;
		
		EntryName.Text = transaction.name;
		DatePickerDate.Date = transaction.date.Date;
		EntryValue.Text = transaction.value.ToString();
	}

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (IsValidData().Result == false) return;
        SaveTransactionInDatabase();

        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);


    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            id = _transaction.id,
            type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            name = EntryName.Text,
            date = DatePickerDate.Date,
            value = Convert.ToDouble(EntryValue.Text)
        };

        //var repository = Handler.MauiContext.Services.GetService<ITransactionRepository>();
        _repository.Update(transaction);
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