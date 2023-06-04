using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Model
{
    public class Transaction
    {
        //[BsonId]
        //OBS precisei deixar os atributos em minusculo visto que o Postgres é case sensitive, ou seja, quando for necessário criar querys no banco seria necessário fazer um scape das letras maisculas.
        public int id { get; set; }
        public TransactionType type { get; set; }
        public string name { get; set; }
        public DateTimeOffset date { get; set; }
        public double value { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expense
    }
}
