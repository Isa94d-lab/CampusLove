namespace CampusLove.Domain.Entities
{
    public enum Reaccion {
        Like,
        Dislike
    };
    public class Interaccion 
    {

        public int Id { get; set;}
        public int UsuarioId { get; set;}
        public int PerfilId { get; set;}
        public Reaccion? Reaccion { get; set;}
    }
}