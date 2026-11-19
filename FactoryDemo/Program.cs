/*
 * name: Eremei Mironov
 * course: ctec 135
 * assignment: programming assignment 6-2, task 2
 * date: 11/18/2026
 */

using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace FactoryDemo;

class Program
{
    static void Main(string[] args)
    {
        // print the title of the program
        Console.WriteLine("***** Factory Model Demo *****");
        Console.WriteLine();

        // hard coded connection string, it points to the AutoLot2023 database on localdb
        string connectionString = @"Data Source=(localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot2023";

        // get the factory for sql server, this factory is the object that makes all the other ado.net objects for us
        DbProviderFactory factory = GetDbProviderFactory("SqlServer");

        // the factory creates a connection object, the using statement makes sure the connection is closed and disposed when the block ends
        using (DbConnection connection = factory.CreateConnection())
        {
            // print the real type of the connection object, for sql server the factory gives us a SqlConnection
            Console.WriteLine($"Your connection object is a: {connection.GetType().Name}");

            // give the connection object the connection string so it knows what server and database to use
            connection.ConnectionString = connectionString;

            // open the connection to the database
            connection.Open();

            // the factory creates a command object, this object will hold our sql query
            DbCommand command = factory.CreateCommand();

            // print the real type of the command object, for sql server it is a SqlCommand
            Console.WriteLine($"Your command object is a: {command.GetType().Name}");

            // attach the open connection to the command so the command can talk to the database
            command.Connection = connection;

            // put the sql query in the command, it selects the id of every car and the name of its make by joining Inventory and Makes
            command.CommandText = "Select i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId";

            // ExecuteReader runs the query and gives back a data reader object with the results, using closes the reader at the end
            using (DbDataReader dataReader = command.ExecuteReader())
            {
                // print the real type of the data reader, for sql server it is a SqlDataReader
                Console.WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");

                // print the title for the list of cars
                Console.WriteLine();
                Console.WriteLine("***** Current Inventory *****");

                // Read moves the reader to the next record and returns false when there are no more records
                while (dataReader.Read())
                {
                    // take the Id and Name columns from the current record and print them
                    Console.WriteLine($"-> Car #{dataReader["Id"]} is a {dataReader["Name"]}.");
                }
            }
        }
    }

    // returns the factory for the provider name, we only use sql server here
    static DbProviderFactory GetDbProviderFactory(string provider)
    {
        // SqlClientFactory.Instance is the one factory object for sql server
        if (provider == "SqlServer")
        {
            return SqlClientFactory.Instance;
        }

        // any other provider name is not supported in this program
        throw new ArgumentException($"Provider {provider} is not supported");
    }
}
