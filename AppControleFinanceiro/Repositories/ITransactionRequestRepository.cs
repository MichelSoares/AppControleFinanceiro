using AppControleFinanceiro.Model;

namespace AppControleFinanceiro.Repositories;

public interface ITransactionRequestRepository
{
    Task AddAsync(Transaction transaction);
    Task DeleteAsync(int id);
    Task<List<Transaction>> GetAllAsync();
    Task UpdateAsync(int id, Transaction transaction);
}
