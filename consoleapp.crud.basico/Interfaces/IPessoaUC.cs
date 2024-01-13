using consoleapp.crud.basico.Entities;

namespace consoleapp.crud.basico.Interfaces
{
    public interface IPessoaUC
    {
        IList<PessoaDepartamento> ListarTodasPessoasDepartamento();

        IList<PessoaEstado> ListarPessoasPorEstado(int idEstado);
    }
}