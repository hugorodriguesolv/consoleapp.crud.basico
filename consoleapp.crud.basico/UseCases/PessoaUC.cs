using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Interfaces;
using consoleapp.crud.basico.Repository;

namespace consoleapp.crud.basico.UseCases
{
    public class PessoaUC : IPessoaUC
    {
        public IList<PessoaDepartamento> ListarTodasPessoas()
        {
            var pessoaRepository = new PessoaRepository();
            var pessoas = pessoaRepository.ObterTodasPessoas();

            return pessoas;
        }

        public IList<PessoaEstado> ListarPessoasPorEstado(int idEstado)
        {
            var pessoaRepository = new PessoaRepository();
            var pessoasEstado = pessoaRepository.ObterPessoasPorEstado(idEstado);

            return pessoasEstado;
        }

        public IList<AlterarDadosPessoa> AlterarDadosPessoais(int idPessoa)
        {

        }
    }
}