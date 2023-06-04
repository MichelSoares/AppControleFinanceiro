using AppControleFinanceiro.Model;

namespace AppControleFinanceiro.Repositories;

public interface ITransactionRequestRepository
{
    void Add(Transaction transaction);
    void Delete(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
    void Update(Transaction transaction);
}
