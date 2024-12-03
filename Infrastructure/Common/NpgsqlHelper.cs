using Npgsql;
namespace Infrastructure.Common;

public static class NpgsqlHelper
{
    public  static NpgsqlConnection CreateConnection(string connectionString)
    {
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}