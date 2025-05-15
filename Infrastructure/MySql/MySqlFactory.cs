using CampusLove.Domain.Entities;
using CampusLove.Domain.Factory;
using CampusLove.Domain.Ports;
using CampusLove.Infrastructure.Repositories;
using MySql.Data.MySqlClient;

namespace CampusLove.Infrastructure.Mysql;

public class MySqlDbFactory : IDbFactory
{
    private readonly string _connectionString;

    public MySqlDbFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

}
