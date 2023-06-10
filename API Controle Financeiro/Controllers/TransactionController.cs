using ControleFinanceiroAPI.Context;
using ControleFinanceiroAPI.Models;
using ControleFinanceiroAPI.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Text.RegularExpressions;

namespace ControleFinanceiroAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[EnableCors("PermitirApiRequest")]
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public TransactionController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public ActionResult ListAllTransaction()
        {
            try
            {
                var listAllTransaction = _context.Transactions.AsNoTracking().ToList();

                if (listAllTransaction is null)
                {
                    UtilHelper.myLogTxtRequest("(ListAllTransaction) - Listando todas as transações (Not Found) ", _config, HttpContext);
                    return NotFound("Sem nenhuma transação.");
                }
                UtilHelper.myLogTxtRequest("(ListAllTransaction) - Listando todas as transações (Ok) ", _config, HttpContext);
                return Ok(listAllTransaction);
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtRequest("(ListAllTransaction) - Listando todas as transações (Internal Server Error) ", _config, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar no servidor \n" + ex.Message + "\n" + ex.StackTrace);

            }
            
        }

        [HttpGet("{id:int}", Name ="ObterTransaction")]
        public ActionResult GetTransaction(int id)
        {
            try
            {
                var transaction =_context.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == id);
                if(transaction is null)
                {
                    UtilHelper.myLogTxtRequest("(GetTransaction) - Busca transação por ID (Not found) ", _config, HttpContext);
                    return NotFound("Transação não encontrada.");
                }

                UtilHelper.myLogTxtRequest("(GetTransaction) - Busca transação por ID (OK) ", _config, HttpContext);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtRequest($"(GetTransaction) - Busca transação por ID: {id} (Internal Server Error) ", _config, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar no servidor \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateTransaction(int id, Transaction transaction)
        {
            try
            {
                if (id != transaction.Id)
                {
                    UtilHelper.myLogTxtRequest($"(UpdateTransaction) - Busca transação por ID (Bad Request) ", _config, HttpContext);
                    return BadRequest("Parametro inválido.");
                }

                _context.Entry(transaction).State = EntityState.Modified;
                _context.SaveChanges();

                UtilHelper.myLogTxtRequest("(UpdateTransaction) - Busca transação por ID (OK) ", _config, HttpContext);
                return Ok();
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtRequest($"(UpdateTransaction) - Atualiza transação ID (Internal Server Error) ", _config, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar no servidor. \n" + ex.Message + "\n" + ex.StackTrace);
            }

            //Transação direta com o banco "TESTE" sem o EF
            //var sql = "UPDATE public.transaction SET type=:TYPE, name=:NAME, date=:DATE, value=:VALUE WHERE id=:ID;";
            //var commad = new NpgsqlCommand(sql);
            //commad.Parameters.Add("TYPE", NpgsqlDbType.Integer);
            //commad.Parameters.Add("NAME", NpgsqlDbType.Varchar);
            //commad.Parameters.Add("DATE", NpgsqlDbType.Timestamp);
            //commad.Parameters.Add("VALUE", NpgsqlDbType.Numeric);

            //commad.Parameters[0].Value = transaction.Type;
            //commad.Parameters[1].Value = transaction.Name;
            //commad.Parameters[2].Value = transaction.Date;
            //commad.Parameters[3].Value = transaction.Value;

            //commad.Prepare();
            //commad.ExecuteNonQuery();
        }

        [HttpDelete]
        public ActionResult DeleteTransaction(int id)
        {
            try
            {
                var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);

                if (transaction is null)
                {
                    UtilHelper.myLogTxtRequest("(DeleteTransaction) - Deleta transação por ID (Not found) ", _config, HttpContext);
                    return NotFound("Transaction Id: " + id + " não encontrado.");
                }

                _context.Remove(transaction);
                _context.SaveChanges();
                UtilHelper.myLogTxtRequest("(DeleteTransaction) - Deleta transação por ID (Ok) ", _config, HttpContext);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtRequest($"(DeleteTransaction) - Deleta transação por ID: {id} (Internal Server Error) ", _config, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar no servidor. \n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult AddTransaction(Transaction transaction)
        {
            try
            {
                if (transaction is null)
                {
                    UtilHelper.myLogTxtRequest("(AddTransaction) - Adiciona transação (Bad request) ", _config, HttpContext);
                    return BadRequest();
                }

                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                UtilHelper.myLogTxtRequest("(AddTransaction) - Adiciona transação (Ok) ", _config, HttpContext);
                return new CreatedAtRouteResult("ObterTransaction", new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                UtilHelper.myLogTxtRequest($"(AddTransaction) - Adiciona transação (Internal Server Error) ", _config, HttpContext);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar no servidor. \n" + ex.Message + "\n" + ex.StackTrace);
            }
            
        }
    }
}
