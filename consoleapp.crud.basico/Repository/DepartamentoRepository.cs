﻿using consoleapp.crud.basico.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.Repository
{
    public class DepartamentoRepository
    {
        private SqlConnection _connection;
        private SqlCommand _command;

        public DepartamentoRepository()
        {
            _connection = new SqlConnection("Data Source=localhost, 1522;Initial Catalog=geekjobs;Integrated Security=False;User ID=sa;Password=AulaGeekJobs1;TrustServerCertificate=true");
            _connection.Open();
        }

        //MODF |  //RETURN METOD        |    //METODO      |
        public IList<DepartamentoCidade> ObterTodosDepartamentos()
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("    Departamento.Id, ");
            sql.AppendLine("    Departamento.Nome, ");
            sql.AppendLine("    Cidade.Nome AS NomeCidade ");
            sql.AppendLine("FROM ");
            sql.AppendLine("    Departamento ");
            sql.AppendLine("    INNER JOIN Cidade ON(Departamento.IdCidade = Cidade.Id) ");

            _command = _connection.CreateCommand();
            _command.CommandText = sql.ToString();
            var dataReader = _command.ExecuteReader();

            var departamentos = new List<Departamento>();

            while (dataReader.Read())
            {
                departamentos.Add(new PessoaDepartamento
                {
                    Id = (int)dataReader["Departamento.Id"],
                    NomeDepartamento = dataReader["Departamento.Nome"].ToString(),
                    NomeCidade = dataReader["NomeCidade"].ToString()
                });
            }

            return departamentos;
        }
    }
}
