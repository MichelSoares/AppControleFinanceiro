using AppControleFinanceiro.DTOs;
using AppControleFinanceiro.Model;
using AppControleFinanceiro.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppControleFinanceiro.Repositories;

public class TransactionRequestRepository : ITransactionRequestRepository
{
    private static string serverAPI = "http://showroom.taassolucoes.com.br:8443/";
    private static TokenJWT tokenJWT = null;
    public UsuarioTokenDTO usuarioTokenDTO;
    private List<Transaction> trans = new List<Transaction>();

    public TransactionRequestRepository()
    {
        usuarioTokenDTO = new UsuarioTokenDTO
        {
            Email = "admin@admin.com",
            Password = "20883"
        };

    }

    public async Task AddAsync(Transaction transaction)
    {
        try
        {
            if (!string.IsNullOrEmpty(tokenJWT.Token))
            {
                var response = await HttpRequestHelper.SendRequestAsyncObject<Transaction>(serverAPI + "Transaction", HttpMethod.Post, tokenJWT.Token, transaction, null, null);
            }
        }
        catch (Exception ex)
        {

            await Console.Out.WriteLineAsync("\n\n" + ex.Message + "\n\n");
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            if (!string.IsNullOrEmpty(tokenJWT.Token))
            {
                var response = await HttpRequestHelper.SendRequestAsyncObject<Transaction>(serverAPI + "Transaction", HttpMethod.Delete, tokenJWT.Token, null, "id", id.ToString());
            }
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync("\n\n" + ex.Message + "\n\n");
        }
    }

    public async Task UpdateAsync(int id, Transaction transaction)
    {
        try
        {
            if (!string.IsNullOrEmpty(tokenJWT.Token))
            {
                var response = await HttpRequestHelper.SendRequestAsyncObject<string>(serverAPI + "Transaction", HttpMethod.Put, tokenJWT.Token, transaction, null, id.ToString());
            }
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync("\n\n" + ex.Message + "\n\n");
        }
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        try
        {
            if (tokenJWT == null || (DateTime.Now.Minute - tokenJWT.Expiration.Minute) >= 120) tokenJWT = await RequestTokenJWT();

            if (!string.IsNullOrEmpty(tokenJWT.Token))
            {
                trans = await HttpRequestHelper.SendRequestAsyncObject<List<Transaction>>(serverAPI + "Transaction", HttpMethod.Get, tokenJWT.Token, null, null);
                return trans;
            }

            return null;
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync("\n\n" + ex.Message);
            return null;

        }

    }

    private async Task<TokenJWT> RequestTokenJWT()
    {
        try
        {
            //var response = await HttpRequestHelper.SendRequestAsync("https://192.168.10.129:7297/Autoriza", HttpMethod.Get, null);
            var response = await HttpRequestHelper.SendRequestAsyncObject<TokenJWT>(serverAPI + "Autoriza/login", HttpMethod.Post, null, usuarioTokenDTO, null, null);
            return response;
        }
        catch (Exception)
        {
            return null;
            throw;
        }
    }
}
