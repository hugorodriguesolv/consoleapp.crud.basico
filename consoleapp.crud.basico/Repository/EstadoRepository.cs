using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Interfaces;
using Microsoft.Data.SqlClient;
using System.Text;

namespace consoleapp.crud.basico.Repository
{
    public class EstadoRepository : IEstadoRepository
    {
        private SqlConnection _connection;
        private SqlCommand _command;

        public EstadoRepository()
        {
            _connection = new SqlConnection("Data Source=localhost, 1523;Initial Catalog=geekjobs;Integrated Security=False;User ID=sa;Password=AulaGeekJobs1;TrustServerCertificate=true");
            _connection.Open();
        }

        public IList<Estado> ObterTodosEstados()
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("    Id,");
            sql.AppendLine("    Nome");
            sql.AppendLine("FROM ");
            sql.AppendLine("    Estado");

            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();
            var dataReader = _command.ExecuteReader();

            var estados = new List<Estado>();

            while (dataReader.Read())
            {
                estados.Add(new Estado
                {
                    Id = (int)dataReader["Id"],
                    Nome = dataReader["Nome"].ToString()
                });
            }

            return estados;
        }
    }
}