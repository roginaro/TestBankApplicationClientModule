using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BankApplicationClientModule.Model;
using Microsoft.EntityFrameworkCore;


namespace BankApplicationClientModule.Crud
{
    public class ClientModuleRepositorie : IClientRepository
    {
        private readonly ClientModuleDBContext _context;
        private readonly string _logFilePath = "client_crud_log.csv";

        public ClientModuleRepositorie(ClientModuleDBContext context)
        {
            _context = context;
        }

        public async Task<List<BankClient>> GetAllClientsThatHaveAtLeastOneAccountAsync()
        {
            var clients = await _context.BankClients
                .Include(c => c.ClientAccounts)
                .Where(c => c.ClientAccounts.Any())
                .ToListAsync();

            Log("READ_ALL_WITH_ACCOUNTS");
            return clients;
        }

        public async Task<BankClient> GetClientByIdAsync(int clientId)
        {
            var client = await _context.BankClients
                .Include(c => c.ClientAccounts)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            Log("READ_BY_ID", clientId);
            return client;
        }

        public async Task AddClientAsync(BankClient client)
        {
            ValidateClient(client);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.BankClients.AddAsync(client);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Log("CREATE", client.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateClientAsync(BankClient client)
        {
            var existing = await _context.BankClients.FindAsync(client.Id);
            if (existing == null)
                throw new InvalidOperationException("Cliente não existe na base.");

            ValidateClient(client);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(existing).CurrentValues.SetValues(client);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Log("UPDATE", client.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteClientAsync(int clientId)
        {
            var client = await _context.BankClients.FindAsync(clientId);
            if (client == null) return;

            _context.BankClients.Remove(client);
            await _context.SaveChangesAsync();

            Log("DELETE", clientId);
        }

        private void ValidateClient(BankClient client)
        {
            if (string.IsNullOrWhiteSpace(client.FirstName))
                throw new ArgumentException("Nome do cliente é obrigatório.");

            if (client.ClientAccounts != null)
            {
                foreach (var account in client.ClientAccounts)
                {
                    //if (account. == DateTime.MinValue)
                    //    throw new ArgumentException("Data de criação da conta é inválida.");
                }
            }
        }

        private void Log(string operation, int? clientId = null)
        {
            var log = $"{DateTime.UtcNow},{operation},{clientId?.ToString() ?? "N/A"}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, log);
        }
    }
}
