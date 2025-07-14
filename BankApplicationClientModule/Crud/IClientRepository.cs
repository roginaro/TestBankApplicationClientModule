// using System.Collections.Generic;
// using System.Threading.Tasks;
// using BankApplicationClientModule.Model;

using BankApplicationClientModule.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankApplicationClientModule.Crud
{
    public interface IClientRepository
    {
        Task<List<BankClient>> GetAllClientsThatHaveAtLeastOneAccountAsync();
        Task<BankClient> GetClientByIdAsync(int clientId);
        Task AddClientAsync(BankClient client);
        Task UpdateClientAsync(BankClient client);
        Task DeleteClientAsync(int clientId);
    }
}
