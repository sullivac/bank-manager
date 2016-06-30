using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BankManager.Models;

namespace BankManager.Controllers
{
    public class AccountTypeController : ApiController
    {
        public IEnumerable<object> Get()
        {
            return Enum.GetValues(typeof(AccountType))
                .OfType<AccountType>()
                .Select(item => new { id = (int)item, name = item.ToString() })
                .ToList();
        }
    }
}