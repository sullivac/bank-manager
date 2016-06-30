using System;
using System.Data;
using System.Data.SQLite;
using System.Web.Configuration;
using BankManager.Models;

namespace BankManager
{
    internal static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = @"create table Customer (
    id text primary key not null,
    firstName text,
    lastName text,
    streetAddress1 text,
    streetAddress2 text,
    streetAddress3 text,
    city text,
    state text,
    zip text,
    mainPhoneNumber text,
    secondaryPhoneNumber text
)";

                command.ExecuteNonQuery();

                command.CommandText = @"create table AccountType (
    id integer primary key not null,
    name text not null
)";

                command.ExecuteNonQuery();

                command.CommandText = @"insert into AccountType (id, name) values (@id, @name)";

                var idParameter = command.CreateParameter();
                idParameter.ParameterName = "@id";
                idParameter.DbType = DbType.Int32;
                command.Parameters.Add(idParameter);

                var nameParameter = command.CreateParameter();
                nameParameter.ParameterName = "@name";
                nameParameter.DbType = DbType.String;
                command.Parameters.Add(nameParameter);

                foreach (AccountType accountType in Enum.GetValues(typeof(AccountType)))
                {
                    idParameter.Value = (int)accountType;
                    nameParameter.Value = accountType.ToString();

                    command.ExecuteNonQuery();
                }

                command.Parameters.Clear();

                command.CommandText = @"create table Account (
    id text primary key not null,
    dateOpened text not null,
    dateClosed text null,
    balance real not null,
    interest real,
    accountTypeId integer not null,
    isOpen integer not null,
    customerId text not null,
    foreign key (customerId) references Customer(id),
    foreign key (accountTypeId) references AccountType(id)
)";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}