using consoleapp.crud.basico.Entities;

namespace consoleapp.crud.basico.Interfaces
{
    public interface IPessoaUC
    {
        IList<Pessoa> ListarTodasPessoas();

        IList<PessoaEstado> ListarPessoasPorEstado(int idEstado);
    }
}