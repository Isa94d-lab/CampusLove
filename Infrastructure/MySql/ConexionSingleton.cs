using System;
using MySql.Data.MySqlClient;
using CampusLove.Application.config.Settings;

namespace CampusLove.Infrastructure.Mysql;

public class ConexionSingleton
{
    private static ConexionSingleton? _instancia;
    private readonly string _connectionString;
    private MySqlConnection? _conexion;

    private ConexionSingleton(string connectionString)
    {
        _connectionString = connectionString;
    }
    
}
