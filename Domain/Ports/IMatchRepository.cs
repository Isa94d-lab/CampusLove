using CampusLove.Domain.Entities;

namespace CampusLove.Domain.Ports;

public interface IMatchRepository
{
    Task<IEnumerable<Match>> ObtenerMatchesDeUsuarioAsync(int perfilId);
    Task CrearMatchAsync(int perfil1, int perfil2); // Solo llamado internamente
}
