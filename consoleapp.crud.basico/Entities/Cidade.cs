namespace consoleapp.crud.basico.Entities
{
    public class Cidade : Entity
    {
        public string? Nome { get; set; }

        public int Populacao { get; set; }

        public string? IdEstado { get; set; }
    }
}