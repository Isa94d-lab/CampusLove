using System;
using CampusLove.Domain.Entities;

namespace CampusLove.Domain.Ports;

public interface IInteresesRepository
{
    Task<IEnumerable<Intereses>> ObtenerTodosAsync();
    Task<Intereses?> ObtenerPorIdAsync(int id);
    Task<bool> AgregarInteresAsync(string tipo);
    Task<bool> EliminarInteresAsync(int id);
}
