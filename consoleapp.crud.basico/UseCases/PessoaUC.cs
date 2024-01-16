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

        public IList<PessoaDepartamento> ListarTodasPessoasDepartamento()
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

        public void InserirPessoa(int idDepartamento, string nome)
        {
            var pessoa = new Pessoa()
            {
                IdDepartamento = idDepartamento,
                Nome = nome
            };

            var pessoaRepository = new PessoaRepository();
            pessoaRepository.InserirPessoa(pessoa);

        }

        public bool ApagarPessoa(int idPessoaInformado)
        {
            var pessoaRepository = new PessoaRepository();
            var apagou = pessoaRepository.ExcluirPessoa(idPessoaInformado) > 0 ? true : false;

            return apagou;

        }

        public bool AlterarDadosPessoas(int idPessoaEscolhida, string novoNomeInformado, int novoIdDepartamento)
        {
            var atualizar = new PessoaRepository();
            bool atualizou = atualizar.AtualizarPessoas(idPessoaEscolhida, novoNomeInformado, novoIdDepartamento) > 0 ? false : true;

            return atualizou;
        }
    }
}