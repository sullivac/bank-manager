using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web.Http;
using BankManager.Models;

namespace BankManager.Controllers
{
    public class TransactionController : ApiController
    {
        public void Post(Guid accountId, Transaction transaction)
        {
            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction dbTransaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = dbTransaction;

                        command.CommandText = "select * from Account where id = @id";
                        command.Parameters.Add("@id", DbType.String).Value = accountId.ToString("B");

                        Account account = null;

                        using (var dataReader = command.ExecuteReader())
                        {
                            var columnNames = new Dictionary<string, int>();

                            for (int x = 0; x < dataReader.FieldCount; x++)
                            {
                                columnNames.Add(dataReader.GetName(x), x);
                            }

                            if (dataReader.Read())
                            {
                                account = new Account
                                {
                                    Id = dataReader.GetGuid(columnNames["id"]),
                                    AccountType = (AccountType)dataReader.GetInt32(columnNames["accountTypeId"]),
                                    Balance = dataReader.GetDouble(columnNames["balance"]),
                                    CustomerId = dataReader.GetGuid(columnNames["customerId"]),
                                    DateClosed = dataReader.IsDBNull(columnNames["dateClosed"]) ? null as DateTime? : dataReader.GetDateTime(columnNames["dateClosed"]),
                                    DateOpened = dataReader.GetDateTime(columnNames["dateOpened"]),
                                    Interest = dataReader.IsDBNull(columnNames["interest"]) ? null as double? : dataReader.GetDouble(columnNames["interest"]),
                                    IsOpen = dataReader.GetBoolean(columnNames["isOpen"])
                                };
                            }
                        }

                        command.CommandText = @"update Account
set balance = @balance
where id = @id";

                        command.Parameters.Add("@id", DbType.String).Value = accountId.ToString("B");

                        command.Parameters.Add("@balance", DbType.Double).Value = account.Balance + transaction.Amount;

                        command.ExecuteNonQuery();
                    }

                    dbTransaction.Commit();
                }

                connection.Close();
            }
        }
    }
}