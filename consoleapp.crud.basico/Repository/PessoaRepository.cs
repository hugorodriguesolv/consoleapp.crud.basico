using consoleapp.crud.basico.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.Repository
{
    public class PessoaRepository
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
    }
}