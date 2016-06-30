using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web.Http;
using BankManager.Models;

namespace BankManager.Controllers
{
    public class CustomerController : ApiController
    {
        public IEnumerable<Customer> Get()
        {
            var result = new List<Customer>();

            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "select * from Customer";

                        using (var dataReader = command.ExecuteReader())
                        {
                            var columnNames = new Dictionary<string, int>();

                            for (int x = 0; x < dataReader.FieldCount; x++)
                            {
                                columnNames.Add(dataReader.GetName(x), x);
                            }

                            while (dataReader.Read())
                            {
                                result.Add(new Customer
                                {
                                    Id = dataReader.GetGuid(columnNames["id"]),
                                    FirstName = dataReader.IsDBNull(columnNames["firstName"]) ? null : dataReader.GetString(columnNames["firstName"]),
                                    LastName = dataReader.IsDBNull(columnNames["lastName"]) ? null : dataReader.GetString(columnNames["lastName"]),
                                    StreetAddress1 = dataReader.IsDBNull(columnNames["streetAddress1"]) ? null : dataReader.GetString(columnNames["streetAddress1"]),
                                    StreetAddress2 = dataReader.IsDBNull(columnNames["streetAddress2"]) ? null : dataReader.GetString(columnNames["streetAddress2"]),
                                    StreetAddress3 = dataReader.IsDBNull(columnNames["streetAddress3"]) ? null : dataReader.GetString(columnNames["streetAddress3"]),
                                    City = dataReader.IsDBNull(columnNames["city"]) ? null : dataReader.GetString(columnNames["city"]),
                                    State = dataReader.IsDBNull(columnNames["state"]) ? null : dataReader.GetString(columnNames["state"]),
                                    Zip = dataReader.IsDBNull(columnNames["zip"]) ? null : dataReader.GetString(columnNames["zip"]),
                                    MainPhoneNumber = dataReader.IsDBNull(columnNames["mainPhoneNumber"]) ? null : dataReader.GetString(columnNames["mainPhoneNumber"]),
                                    SecondaryPhoneNumber = dataReader.IsDBNull(columnNames["secondaryPhoneNumber"]) ? null : dataReader.GetString(columnNames["secondaryPhoneNumber"])
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

        public Customer Get(Guid id)
        {
            Customer result = null;

            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = "select * from Customer where id = @id";
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
                                result = new Customer
                                {
                                    Id = dataReader.GetGuid(columnNames["id"]),
                                    FirstName = dataReader.IsDBNull(columnNames["firstName"]) ? null : dataReader.GetString(columnNames["firstName"]),
                                    LastName = dataReader.IsDBNull(columnNames["lastName"]) ? null : dataReader.GetString(columnNames["lastName"]),
                                    StreetAddress1 = dataReader.IsDBNull(columnNames["streetAddress1"]) ? null : dataReader.GetString(columnNames["streetAddress1"]),
                                    StreetAddress2 = dataReader.IsDBNull(columnNames["streetAddress2"]) ? null : dataReader.GetString(columnNames["streetAddress2"]),
                                    StreetAddress3 = dataReader.IsDBNull(columnNames["streetAddress3"]) ? null : dataReader.GetString(columnNames["streetAddress3"]),
                                    City = dataReader.IsDBNull(columnNames["city"]) ? null : dataReader.GetString(columnNames["city"]),
                                    State = dataReader.IsDBNull(columnNames["state"]) ? null : dataReader.GetString(columnNames["state"]),
                                    Zip = dataReader.IsDBNull(columnNames["zip"]) ? null : dataReader.GetString(columnNames["zip"]),
                                    MainPhoneNumber = dataReader.IsDBNull(columnNames["mainPhoneNumber"]) ? null : dataReader.GetString(columnNames["mainPhoneNumber"]),
                                    SecondaryPhoneNumber = dataReader.IsDBNull(columnNames["secondaryPhoneNumber"]) ? null : dataReader.GetString(columnNames["secondaryPhoneNumber"])
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

        public void Post(Customer customer)
        {
            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"insert into Customer (
    id,
    firstName,
    lastName,
    streetAddress1,
    streetAddress2,
    streetAddress3,
    city,
    state,
    zip,
    mainPhoneNumber,
    secondaryPhoneNumber
)
values (
    @id,
    @firstName,
    @lastName,
    @streetAddress1,
    @streetAddress2,
    @streetAddress3,
    @city,
    @state,
    @zip,
    @mainPhoneNumber,
    @secondaryPhoneNumber
)";

                        command.Parameters.Add("@id", DbType.String).Value = Guid.NewGuid().ToString("B");

                        if (customer.FirstName == null)
                            command.Parameters.Add("@firstName", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@firstName", DbType.String).Value = customer.FirstName;

                        if (customer.LastName == null)
                            command.Parameters.Add("@lastName", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@lastName", DbType.String).Value = customer.LastName;

                        if (customer.StreetAddress1 == null)
                            command.Parameters.Add("@streetAddress1", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress1", DbType.String).Value = customer.StreetAddress1;

                        if (customer.StreetAddress2 == null)
                            command.Parameters.Add("@streetAddress2", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress2", DbType.String).Value = customer.StreetAddress2;

                        if (customer.StreetAddress3 == null)
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = customer.StreetAddress3;

                        if (customer.StreetAddress3 == null)
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = customer.StreetAddress3;

                        if (customer.City == null)
                            command.Parameters.Add("@city", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@city", DbType.String).Value = customer.City;

                        if (customer.State == null)
                            command.Parameters.Add("@state", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@state", DbType.String).Value = customer.State;

                        if (customer.Zip == null)
                            command.Parameters.Add("@zip", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@zip", DbType.String).Value = customer.Zip;

                        if (customer.MainPhoneNumber == null)
                            command.Parameters.Add("@mainPhoneNumber", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@mainPhoneNumber", DbType.String).Value = customer.MainPhoneNumber;

                        if (customer.SecondaryPhoneNumber == null)
                            command.Parameters.Add("@secondaryPhoneNumber", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@secondaryPhoneNumber", DbType.String).Value = customer.SecondaryPhoneNumber;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        public void Put(Guid id,Customer customer)
        {
            using (var connection = new SQLiteConnection(Startup.ApplicationConnectionString))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"update Customer 
set
    id                   = @id,
    firstName            = @firstName,
    lastName             = @lastName,
    streetAddress1       = @streetAddress1,
    streetAddress2       = @streetAddress2,
    streetAddress3       = @streetAddress3,
    city                 = @city,
    state                = @state,
    zip                  = @zip,
    mainPhoneNumber      = @mainPhoneNumber,
    secondaryPhoneNumber = @secondaryPhoneNumber
where id = @id";

                        command.Parameters.Add("@id", DbType.String).Value = id.ToString("B");

                        if (customer.FirstName == null)
                            command.Parameters.Add("@firstName", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@firstName", DbType.String).Value = customer.FirstName;

                        if (customer.LastName == null)
                            command.Parameters.Add("@lastName", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@lastName", DbType.String).Value = customer.LastName;

                        if (customer.StreetAddress1 == null)
                            command.Parameters.Add("@streetAddress1", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress1", DbType.String).Value = customer.StreetAddress1;

                        if (customer.StreetAddress2 == null)
                            command.Parameters.Add("@streetAddress2", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress2", DbType.String).Value = customer.StreetAddress2;

                        if (customer.StreetAddress3 == null)
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = customer.StreetAddress3;

                        if (customer.StreetAddress3 == null)
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@streetAddress3", DbType.String).Value = customer.StreetAddress3;

                        if (customer.City == null)
                            command.Parameters.Add("@city", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@city", DbType.String).Value = customer.City;

                        if (customer.State == null)
                            command.Parameters.Add("@state", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@state", DbType.String).Value = customer.State;

                        if (customer.Zip == null)
                            command.Parameters.Add("@zip", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@zip", DbType.String).Value = customer.Zip;

                        if (customer.MainPhoneNumber == null)
                            command.Parameters.Add("@mainPhoneNumber", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@mainPhoneNumber", DbType.String).Value = customer.MainPhoneNumber;

                        if (customer.SecondaryPhoneNumber == null)
                            command.Parameters.Add("@secondaryPhoneNumber", DbType.String).Value = DBNull.Value;
                        else
                            command.Parameters.Add("@secondaryPhoneNumber", DbType.String).Value = customer.SecondaryPhoneNumber;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

                connection.Close();
            }
        }
    }
}