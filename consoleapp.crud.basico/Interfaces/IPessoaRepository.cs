using consoleapp.crud.basico.Entities;

namespace consoleapp.crud.basico.Interfaces
{
    public interface IPessoaRepository
    {
        IList<PessoaDepartamento> ObterTodasPessoas();

        IList<PessoaEstado> ObterPessoasPorEstado(int IdEstado);
    }
}