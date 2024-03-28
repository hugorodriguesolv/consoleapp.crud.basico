using consoleapp.crud.basico.Entities;

namespace consoleapp.crud.basico.Interfaces
{
    public interface IEstadoRepository
    {
        IList<Estado> ObterTodosEstados();
    }
}