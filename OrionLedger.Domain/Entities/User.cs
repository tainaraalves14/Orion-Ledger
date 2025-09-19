using System;

namespace OrionLedger.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Armazena o hash da senha
        public decimal Balance { get; set; } = 0; // Saldo inicial é 0
        public string Role { get; set; } = "User"; // "Admin" ou "User"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

       

   
    }
}