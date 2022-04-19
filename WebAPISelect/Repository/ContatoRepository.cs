using System.Collections.Generic;
using WebAPISelect.Model;
using MySql.Data.MySqlClient;
using Dapper;

namespace WebAPISelect.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly string _connectionString;

        public ContatoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Contato> Delete(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "DELETE FROM Contato WHERE id = @Id";

                return connection.Query<Contato>(sqlStatement, new { Id = id });
            }
   
        }

        public IEnumerable<Contato> Get(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "SELECT * FROM Contato WHERE id = @Id";

                return connection.Query<Contato>(sqlStatement, new { Id = id });
            }
        }

        //Criação da conexão com o banco e retorno de uma consulta
        public IEnumerable<Contato> GetAll()
        {
            using(MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                return connection.Query<Contato>("SELECT * FROM Contato");
            }
        }

        public IEnumerable<Contato> Insert(string name, string email, string number)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "INSERT INTO Contato (name, email, number) VALUES (@Name, @Email, @Number)";

                return connection.Query<Contato>(sqlStatement, new { Name = name, Email = email, Number = number});
            }

        }

        public IEnumerable<Contato> Update(int id, string name, string email, string number)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var sqlStatement = "UPDATE Contato SET name = @Name, email = @Email, number = @Number WHERE id = @Id";

                return connection.Query<Contato>(sqlStatement, new { Id = id, Name = name, Email = email, Number = number });
            }
        }
    }
}

