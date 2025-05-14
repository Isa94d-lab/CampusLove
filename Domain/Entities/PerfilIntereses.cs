namespace CampusLove.Domain.Entities
{
    public class PerfilIntereses 
    {
        public int Id { get; set;}
        public int PerfilId { get; set;}
        public int InteresesId { get; set;}
        public Perfil? Perfil { get; set; }
        public Intereses? Intereses { get; set; }
    }
}