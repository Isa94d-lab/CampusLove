using CampusLove.Domain.Factory;
using CampusLove.Domain.Ports;
using CampusLove.Infrastructure.Repositories;

namespace CampusLove.Infrastructure.Mysql;

public class MySqlDbFactory : IDbFactory
{
    private readonly string _connectionString;

    public MySqlDbFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IEstadoPerfilRepository CreateEstadoPerfilRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new EstadoPerfilRepository(connection);
    }
    public IGeneroRepository CreateGeneroRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new GeneroRepository(connection);
    }
    public IInteraccionRepository CreateInteraccionRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new InteraccionRepository(connection);
    }
    public ILikesDiariosRepository CreateLikesDiariosRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new LikesDiariosRepository(connection);
    }
    public IMatchRepository CreateMatchRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new MatchRepository(connection);
    }
    public IPerfilInteresesRepository CreatePerfilInteresesRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new PerfilInteresesRepository(connection);
    }
    public IPerfilRepository CreatePerfilRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new PerfilRepository(connection);
    }
    public IProfesionRepository CreateProfesionRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new ProfesionRepository(connection);
    }
    public IUsuarioRepository CreateUsuarioRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new UsuarioRepository(connection);
    }

    public IInteresesRepository CreateInteresesRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new InteresesRepository(connection);
    }

    public IEstadisticasRepository CreateEstadisticasRepository()
    {
        var connection = ConexionSingleton.Instancia(_connectionString).ObtenerConexion();
        return new EstadisticasRepository(connection);
    }



}
