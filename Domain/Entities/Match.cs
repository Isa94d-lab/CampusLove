namespace CampusLove.Domain.Entities
{
    public class Match 
    {
        public int Id { get; set;}
        public int Perfil1Id { get; set;}
        public int Perfil2Id { get; set;}
        public DateTime Fecha { get; set;}
        public Perfil? Perfil { get; set;}
    }
}