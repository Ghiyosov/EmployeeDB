using Npgsql;

namespace Infrastructure.NpqsqlHellpers;

public  class NpgsqlHellpers
{
    
    public readonly string conectionString = @"Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=123456;";
    public const string select = "select * from employee";
    
    
}