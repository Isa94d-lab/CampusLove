namespace CampusLove.Domain.Entities
{
    public class LikesDriarios 
    {
        public int Id { get; set;}
        public int PerfilId { get; set;}
        public int Cantidad { get; set;}
        public Perfil? Perfil { get; set;}

    }
}