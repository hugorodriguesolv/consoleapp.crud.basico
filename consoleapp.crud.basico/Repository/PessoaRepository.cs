using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Interfaces;
using Microsoft.Data.SqlClient;
using System.Text;

namespace consoleapp.crud.basico.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private SqlConnection _connection;
        private SqlCommand _command;

        public PessoaRepository()
        {
            _connection = new SqlConnection("Data Source=localhost, 1522;Initial Catalog=geekjobs;Integrated Security=False;User ID=sa;Password=AulaGeekJobs1;TrustServerCertificate=true");
            _connection.Open();
        }

        public IList<PessoaDepartamento> ObterTodasPessoas()
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("    Pessoa.Id AS IdPessoa,");
            sql.AppendLine("    Pessoa.Nome AS NomePessoa,");
            sql.AppendLine("    Departamento.Nome AS NomeDepartamento");
            sql.AppendLine(" FROM ");
            sql.AppendLine("    Pessoa");
            sql.AppendLine("    INNER JOIN Departamento ON (Pessoa.IdDepartamento = Departamento.Id)");

            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();
            var dataReader = _command.ExecuteReader();

            var pessoas = new List<PessoaDepartamento>();

            while (dataReader.Read())
            {
                pessoas.Add(new PessoaDepartamento
                {
                    Id = (int)dataReader["IdPessoa"],
                    NomePessoa = dataReader["NomePessoa"].ToString(),
                    NomeDepartamento = dataReader["NomeDepartamento"].ToString()
                });
            }

            return pessoas;
        }

        public IList<PessoaEstado> ObterPessoasPorEstado(int IdEstado)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("    Pessoa.Id as IdPessoa,");
            sql.AppendLine("    Pessoa.Nome as NomePessoa,");
            sql.AppendLine("    Departamento.Nome as NomeDepartamento,");
            sql.AppendLine("    Estado.Nome as NomeEstado");
            sql.AppendLine(" FROM ");
            sql.AppendLine("    Pessoa");
            sql.AppendLine("    inner join Departamento on (Pessoa.IdDepartamento = Departamento.Id)");
            sql.AppendLine("    inner join Cidade on (Departamento.IdCidade = Cidade.Id)");
            sql.AppendLine("    inner join Estado on (Cidade.IdEstado = Estado.Id)");
            sql.AppendLine(" WHERE ");
            sql.AppendLine("    Estado.Id = @IdEstado");

            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();

            _command.Parameters.Add("@IdEstado", System.Data.SqlDbType.Int);
            _command.Parameters["@IdEstado"].Value = IdEstado;

            var dataReader = _command.ExecuteReader();

            var retorno = new List<PessoaEstado>();

            while (dataReader.Read())
            {
                retorno.Add(new PessoaEstado
                {
                    Id = (int)dataReader["IdPessoa"],
                    NomePessoa = dataReader["NomePessoa"].ToString(),
                    NomeDepartamento = dataReader["NomeDepartamento"].ToString(),
                    NomeEstado = dataReader["NomeEstado"].ToString()
                });
            }

            return retorno;
        }

        public int ExcluirPessoa(int idPessoa)
        {
            var sql = new StringBuilder();
            sql.AppendLine("DELETE Pessoa WHERE Id = @Id ");
            
            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();

            _command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            _command.Parameters["@Id"].Value = idPessoa;

            var linhasAfetadas = _command.ExecuteNonQuery();

            return linhasAfetadas;
        }

        internal int AtualizarPessoas(int idNomeAlterado, string novoNome)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE ");
            sql.AppendLine("    Pessoa ");
            sql.AppendLine("SET ");
            sql.AppendLine("    Pessoa.Nome = @NovoNome ");
            sql.AppendLine("WHERE    ");
            sql.AppendLine("    Id = @Id ");

            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();

            _command.Parameters.Add("@NovoNome", System.Data.SqlDbType.Text).Value = novoNome;
            _command.Parameters["@NovoNome"].Value = novoNome;

            _command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
            _command.Parameters["@Id"].Value = idNomeAlterado;

            var linhasAfetadas = _command.ExecuteNonQuery();

            return linhasAfetadas;

        }

    }
}