using System;
using Npgsql;

class Program
{
    static void Main()
    {
        // Connection string para o seu PostgreSQL no Docker
        var connString = "Host=127.0.0.1;Port=5432;Database=orion_ledger;Username=orion;Password=orion123;SSL Mode=Disable";

        using var conn = new NpgsqlConnection(connString);

        try
        {
            conn.Open();
            Console.WriteLine("✅ Conexão bem-sucedida!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Falha na conexão:");
            Console.WriteLine(ex.Message);
        }
    }
}
