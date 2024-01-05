using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Interfaces;
using consoleapp.crud.basico.Repository;

namespace consoleapp.crud.basico.UseCases
{
    public class PessoaUC : IPessoaUC
    {
        public IList<Departamento> ListarTodasPessoas()
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

<<<<<<< HEAD
        public void AlterarDadosPessoais(Pessoa pessoa)
        {
            var pessoaRepository = new PessoaRepository();
            pessoaRepository.AlterarDadosPessoais(idPessoa);

        }
=======
        public bool ApagarPessoa(int idPessoaInformado)
        {
            var pessoaRepository = new PessoaRepository();
            var apagou = pessoaRepository.ExcluirPessoa(idPessoaInformado) > 0 ? true : false;

            return apagou;

        }

>>>>>>> feature/excluir-pessoas
    }
}