namespace CampusLove.Domain.Entities
{
    public class Usuario 
    {
        public int Id { get; set;}
        public int PerfilId { get; set;}
        public string? Nickname { get; set;}
        public string? Password { get; set;}
    }
}