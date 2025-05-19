using System;
using CampusLove.Domain.Ports;

namespace CampusLove.Domain.Factory;

public interface IDbFactory
{
    IEstadoPerfilRepository CreateEstadoPerfilRepository();
    IGeneroRepository CreateGeneroRepository();
    IInteraccionRepository CreateInteraccionRepository();
    ILikesDiariosRepository CreateLikesDiariosRepository();
    IMatchRepository CreateMatchRepository();
    IPerfilInteresesRepository CreatePerfilInteresesRepository();
    IPerfilRepository CreatePerfilRepository();
    IProfesionRepository CreateProfesionRepository();
    IUsuarioRepository CreateUsuarioRepository();
    IInteresesRepository CreateInteresesRepository();
    IEstadisticasRepository CreateEstadisticasRepository();
    
}