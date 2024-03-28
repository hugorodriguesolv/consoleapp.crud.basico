using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Interfaces;
using consoleapp.crud.basico.Repository;

namespace consoleapp.crud.basico.UseCases
{
    public class EstadoUC : IEstadoUC
    {
        public IList<Estado> ListarTodosEstados()
        {
            var estadoRepository = new EstadoRepository();
            var estados = estadoRepository.ObterTodosEstados();

            return estados;
        }
    }
}