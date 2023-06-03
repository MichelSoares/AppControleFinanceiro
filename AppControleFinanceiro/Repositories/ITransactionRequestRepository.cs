using AppControleFinanceiro.Model;

namespace AppControleFinanceiro.Repositories;

public interface ITransactionRequestRepository
{
    void Add(Transaction transaction);
    void Delete(Transaction transaction);
    List<Transaction> GetAll();
    void Update(Transaction transaction);
}
