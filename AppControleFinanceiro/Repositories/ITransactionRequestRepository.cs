using AppControleFinanceiro.Model;

namespace AppControleFinanceiro.Repositories;

public interface ITransactionRequestRepository
{
    Task AddAsync(Transaction transaction);
    Task DeleteAsync(Transaction transaction);
    Task<List<Transaction>> GetAllAsync();
    Task UpdateAsync(Transaction transaction);
}
