using AppControleFinanceiro.Model;
using AppControleFinanceiro.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Repositories;

public class TransactionRequestRepository : ITransactionRequestRepository
{
    private static string Token = string.Empty;

    public void Add(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public void Delete(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public List<Transaction> GetAll()
    {
        List<Transaction> trans = new List<Transaction>();
        var teste = RequestTokenJWT();
        //HttpRequestHelper.SendRequestAsync();
        //throw new NotImplementedException();
        return trans;
    }

    public void Update(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    private async Task<string> RequestTokenJWT()
    {
        try
        {
            var response = await HttpRequestHelper.SendRequestAsync("https://192.168.10.129:7297/Autoriza", HttpMethod.Get, null);
            return response;
        }
        catch (Exception)
        {
            return null;
            throw;
        }
    }
}
