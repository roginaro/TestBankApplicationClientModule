using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BankApplicationClientModule.Model;
using Microsoft.EntityFrameworkCore;


namespace BankApplicationClientModule.Crud
{
    public class ClientModuleRepositorie : ClientModuleDataAccess
    {
        public ClientModuleRepositorie(): base()
        {
        }

        public async Task<List<BankClient>> GetAllClientsThatHaveAtLeastOneAccountAsync()
        {
            var clients = await DBContext.BankClients
                .Include(c => c.ClientAccounts)
                .Where(c => c.ClientAccounts.Any())
                .ToListAsync();


            foreach (var c in clients)
            {
                Console.WriteLine($"Client: {c.FirstName}, Accounts: {c.ClientAccounts.Count}");
            }


            return clients;
        }

        public bool SaveNewClient(BankClient client)
        {
            try
            {
                DBContext.BankClients.Add(client);
                DBContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar cliente: {ex.Message}");
                return false;
            }
        }

        public BankClient StartTracking(BankClient client)
        {
            var entry = DBContext.Entry(client);

            if (entry.State != EntityState.Detached)
                return client;

            var local = DBContext.BankClients.Local.FirstOrDefault(c => c.Id == client.Id);
            if (local != null)
            {
                DBContext.Entry(local).State = EntityState.Detached;
            }

            DBContext.Attach(client);

            if (client.Id > 0)
                DBContext.Entry(client).State = EntityState.Modified;
            else
                DBContext.Entry(client).State = EntityState.Added;

            return client;
        }

        public bool IsClientTrackedByEF(BankClient client)
        {
            return DBContext.ChangeTracker.Entries<BankClient>()
            .Any(e => e.Entity == client);
        }
    }
}
