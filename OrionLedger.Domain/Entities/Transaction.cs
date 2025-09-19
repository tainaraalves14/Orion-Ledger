using System;

namespace OrionLedger.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; } // Usuário que envia
        public Guid ToUserId { get; set; } // Usuário que recebe
        public decimal Amount { get; set; } // Valor da transação
        public DateTime Date { get; set; } = DateTime.UtcNow; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public string Status { get; set; } = "Pending"; // Pending/Completed/Failed
    }   
}