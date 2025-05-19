using CampusLove.Domain.Entities;

namespace CampusLove.Domain.Ports;

public interface IPerfilInteresesRepository
{
    Task<IEnumerable<Intereses>> ObtenerInteresesDePerfilAsync(int perfilId);
    Task<bool> AgregarInteresAsync(int perfilId, int interesId);
    Task<bool> EliminarInteresAsync(int perfilId, int interesId);
}
