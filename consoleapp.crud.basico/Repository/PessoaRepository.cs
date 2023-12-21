using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private SqlConnection _connection;
        private SqlCommand _command;

        public PessoaRepository()
        {
            _connection = new SqlConnection("Data Source=sql-server, 1433;Initial Catalog=geekjobs;Integrated Security=False;User ID=sa;Password=AulaGeekJobs1;TrustServerCertificate=true");
            _connection.Open();
        }

        public IList<Pessoa> ObterTodasPessoas()
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("    Id,");
            sql.AppendLine("    Nome,");
            sql.AppendLine("    IdDepartamento");
            sql.AppendLine(" FROM ");
            sql.AppendLine("    Pessoa");

            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();
            var dataReader = _command.ExecuteReader();

            var retorno = new List<Pessoa>();

            while (dataReader.Read())
            {
                retorno.Add(new Pessoa
                {
                    Id = (int)dataReader["Id"],
                    Nome = dataReader["Nome"].ToString(),
                    IdDepartamento = (int)dataReader["IdDepartamento"],
                });
            }

            return retorno;
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
            sql.AppendLine("    inner join Departamento on(Pessoa.IdDepartamento = Departamento.Id)");
            sql.AppendLine("    inner join Cidade on(Departamento.IdCidade = Cidade.Id)");
            sql.AppendLine("    inner join Estado on(Cidade.IdEstado = Estado.Id)");
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
    }
}