using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web.Http;
using BankManager.Models;

namespace BankManager.Controllers
{
    public class HealthSavingsAccountController : ApiController
    {
        public IEnumerable<Account> Get()
        {
            var result = new List<Account>();

            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "select * from Account where accountTypeId = @accountTypeId";
                        command.Parameters.Add("@accountTypeId", DbType.Int32).Value = (int)AccountType.HealthSavings;

                        using (var dataReader = command.ExecuteReader())
                        {
                            var columnNames = new Dictionary<string, int>();

                            for (int x = 0; x < dataReader.FieldCount; x++)
                            {
                                columnNames.Add(dataReader.GetName(x), x);
                            }

                            while (dataReader.Read())
                            {
                                result.Add(new Account
                                {
                                    Id = dataReader.GetGuid(columnNames["id"]),
                                    AccountType = (AccountType)dataReader.GetInt32(columnNames["accountTypeId"]),
                                    Balance = dataReader.GetDouble(columnNames["balance"]),
                                    CustomerId = dataReader.GetGuid(columnNames["customerId"]),
                                    DateClosed = dataReader.IsDBNull(columnNames["dateClosed"]) ? null as DateTime? : dataReader.GetDateTime(columnNames["dateClosed"]),
                                    DateOpened = dataReader.GetDateTime(columnNames["dateOpened"]),
                                    Interest = dataReader.IsDBNull(columnNames["interest"]) ? null as double? : dataReader.GetDouble(columnNames["interest"]),
                                    IsOpen = dataReader.GetBoolean(columnNames["isOpen"])
                                });
                            }
                        }
                    }

                    transaction.Rollback();
                }

                connection.Close();
            }

            return result;
        }

        public Account Get(Guid id)
        {
            Account result = null;

            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "select * from Account where id = @id";
                        command.Parameters.Add("@id", DbType.String).Value = id.ToString("B");

                        using (var dataReader = command.ExecuteReader())
                        {
                            var columnNames = new Dictionary<string, int>();

                            for (int x = 0; x < dataReader.FieldCount; x++)
                            {
                                columnNames.Add(dataReader.GetName(x), x);
                            }

                            if (dataReader.Read())
                            {
                                result = new Account
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
                    }

                    transaction.Rollback();
                }

                connection.Close();
            }

            return result;
        }

        public void Post(Account account)
        {
            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"insert into Account (
    id,
    dateOpened,
    dateClosed,
    balance,
    isOpen,
    accountTypeId,
    customerId
)
values (
    @id,
    @dateOpened,
    @dateClosed,
    @balance,
    @isOpen,
    @accountTypeId,
    @customerId
)";

                        command.Parameters.Add("@id", DbType.String).Value = Guid.NewGuid().ToString("B");
                        command.Parameters.Add("@dateOpened", DbType.String).Value = DateTime.UtcNow.ToString("o");

                        if (account.DateClosed.HasValue)
                            command.Parameters.Add("@dateClosed", DbType.String).Value = account.DateClosed.Value.ToUniversalTime().ToString("o");
                        else
                            command.Parameters.Add("@dateClosed", DbType.String).Value = DBNull.Value;

                        command.Parameters.Add("@balance", DbType.Double).Value = account.Balance;
                        command.Parameters.Add("@isOpen", DbType.Int32).Value = 1;
                        command.Parameters.Add("@accountTypeId", DbType.Int32).Value = (int)AccountType.HealthSavings;
                        command.Parameters.Add("@customerId", DbType.String).Value = account.CustomerId.ToString("B");

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        public void Put(Guid id, Account account)
        {
            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"update Account 
set
    dateClosed    = @dateClosed,
    balance       = @balance,
    isOpen        = @isOpen
where id = @id";

                        command.Parameters.Add("@id", DbType.String).Value = id.ToString("B");

                        if (account.DateClosed.HasValue)
                            command.Parameters.Add("@dateClosed", DbType.String).Value = account.DateClosed.Value.ToUniversalTime().ToString("o");
                        else
                            command.Parameters.Add("@dateClosed", DbType.String).Value = DBNull.Value;

                        command.Parameters.Add("@balance", DbType.Double).Value = account.Balance;
                        command.Parameters.Add("@isOpen", DbType.Int32).Value = 1;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

                connection.Close();
            }
        }
    }
}