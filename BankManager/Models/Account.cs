using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankManager.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime? DateClosed { get; set; }

        public double Balance { get; set; }

        public double? Interest { get; set; }

        public bool IsOpen { get; set; }

        public AccountType AccountType { get; set; }

        public Guid CustomerId { get; set; }
    }
}
