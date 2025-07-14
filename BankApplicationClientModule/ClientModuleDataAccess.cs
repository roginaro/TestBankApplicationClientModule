using BankApplicationClientModule.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankApplicationClientModule
{
    public class ClientModuleDataAccess : BaseClientModuleDataAccess
    {

        public ClientModuleDataAccess() : base()
        {
        }

        /// <summary>
        /// TODO: change this function to meet requirements.
        /// </summary>
        /// <returns></returns>
        public IList<BankClient> GetAllClientsThatHaveAtLeastOneAccountDetached()
        {
            return DBContext.BankClients.ToList();
        }

        /// <summary>
        /// TODO: implement this function to meet requirements.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool SaveNewClient(BankClient client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: implement this function to meet requirements.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public BankClient StartTracking(BankClient client)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO: implement this function to meet requirements.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsClientTrackedByEF(BankClient client)
        {
            throw new NotImplementedException();
        }
    }
}
