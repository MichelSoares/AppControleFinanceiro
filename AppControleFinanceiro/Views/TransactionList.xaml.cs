using AppControleFinanceiro.Model;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{

    private readonly ITransactionRepository _transactionRepositor;
    private readonly ITransactionRequestRepository _transactionRequestRepository;


    public TransactionList(ITransactionRepository transactionRepository, ITransactionRequestRepository transactionRequestRepository)
    {
        _transactionRepositor = transactionRepository;
        _transactionRequestRepository = transactionRequestRepository;
        InitializeComponent();

        Reload();

        WeakReferenceMessenger.Default.Register(this, (MessageHandler<object, string>)((e, msg) =>
        {
            Reload();
        }));
        
    }

    private async Task Reload()
    {
        /*var items = _transactionRepositor.GetAll();
        CollectionViewTransaction.ItemsSource = items;

         double income = items.Where(a => a.Type == Model.TransactionType.Income).Sum(a => a.Value);
         double expense = items.Where(a => a.Type == Model.TransactionType.Expense).Sum(a => a.Value);

        double balance = income - expense;

        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text= expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");*/

        var items = await _transactionRequestRepository.GetAllAsync();
        CollectionViewTransaction.ItemsSource = items;

    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs e)
    {
        //Publisher - Subscribers
        //TransactionAdd - Publisher > Cadastro
        //TransactionList - Subscriber


        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
        //App.Current.MainPage = new TransactionAdd();
    }

    //private void OnButtonClicked_To_TransactionEdit(object sender, EventArgs e)
    //{

    //    var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
    //    Navigation.PushModalAsync(transactionEdit);
    //    //App.Current.MainPage = new TransactionEdit();
    //}

    private void TapGestureRecognizerTapped_to_TransactionEdit(object sender, TappedEventArgs e)
    {
        //Transaction transaction = (Transaction) ((TapGestureRecognizer) sender).CommandParameter;

        var grid = (Grid) sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction) gesture.CommandParameter;

        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_ToDelete(object sender, TappedEventArgs e)
    {
        await AnimationBorder((Border)sender, true);
        bool result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem certeza que deseja excluir?", "Sim", "Não");

        if (result)
        {
            Transaction transaction = (Transaction)e.Parameter;
            //_transactionRepositor.Delete((Transaction)sender);
            _transactionRepositor.Delete(transaction);

            Reload();
        }
        else
        {
            await AnimationBorder((Border)sender, false);
        }
    }

    private Color _borderOriginalBackgroundColor;
    private string _labelOriginalText;
    private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
    {
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalText = label.Text;
            await border.RotateYTo(90, 500);
            border.BackgroundColor = Colors.Red;
            label.TextColor = Colors.White;
            label.Text = "X";
            await border.RotateYTo(180, 500);
        }
        else
        {
            await border.RotateYTo(90, 500);
            border.BackgroundColor = _borderOriginalBackgroundColor;
            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;
            await border.RotateYTo(0, 500);
        }
    }
}