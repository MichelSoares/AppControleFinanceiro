using AppControleFinanceiro.DTOs;
using AppControleFinanceiro.Model;
using AppControleFinanceiro.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Repositories;

public class TransactionRequestRepository : ITransactionRequestRepository
{
    private static string serverAPI = "http://showroom.taassolucoes.com.br:8443/";
    private static string Token = string.Empty;
    public UsuarioTokenDTO usuarioTokenDTO;

    public TransactionRequestRepository()
    {
        usuarioTokenDTO = new UsuarioTokenDTO
        {
            Email = "admin@admin.com",
            Password = "20883"
        };
    }

    public void Add(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public void Delete(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        try
        {
            List<Transaction> trans = new List<Transaction>();
            Token = await RequestTokenJWT();

            if (!string.IsNullOrEmpty(Token))
            {
                trans = await HttpRequestHelper.SendRequestAsyncObject<List<Transaction>>(serverAPI + "Transaction", HttpMethod.Get, Token, null);
            }

            return trans;
        }
        catch (Exception)
        {
            return null;
            throw;
        }
        
    }


    private async Task<string> RequestTokenJWT()
    {
        try
        {
            //var response = await HttpRequestHelper.SendRequestAsync("https://192.168.10.129:7297/Autoriza", HttpMethod.Get, null);
            var response = await HttpRequestHelper.SendRequestAsyncObject<string>(serverAPI + "Autoriza/login", HttpMethod.Post, null, usuarioTokenDTO);
            return response;
        }
        catch (Exception)
        {
            return null;
            throw;
        }
    }

    void ITransactionRequestRepository.Update(Transaction transaction)
    {
        throw new NotImplementedException();
    }
}
