using System.Text.Json.Serialization;

namespace AppControleFinanceiro.Model
{
    public class Transaction
    {
        //[BsonId]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public TransactionType Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public enum TransactionType
    {
        Income = 1,
        Expense = 2
    }
}
