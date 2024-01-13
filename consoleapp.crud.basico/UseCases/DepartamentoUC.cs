using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Repository;

namespace consoleapp.crud.basico.UseCases
{
    public class DepartamentoUC
    {
        public IList<DepartamentoCidade> ListarTodosDepartamentos()
        {
            var departamentoRepository = new DepartamentoRepository();
            var departamentos = departamentoRepository.ObterTodosDepartamentos();

            return departamentos;
        }
    }
}