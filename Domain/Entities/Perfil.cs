namespace CampusLove.Domain.Entities
{
    public class Perfil 
    {
        public int Id { get; set;}
        public int ProfesionId { get; set;}
        public int GeneroId { get; set;}
        public int EstadoPerfilId { get; set;}
        public string? Nombre { get; set;}
        public string? Apellido { get; set;}
        public int Edad { get; set;}
        public string? Frase { get; set;}
        public string? Gustos { get; set;}
        public int Coins { get; set;}
        public Profesion? Profesion { get; set; }
        public Genero? Genero { get; set; }
        public EstadoPerfil? EstadoPerfil { get; set; }

    }
}