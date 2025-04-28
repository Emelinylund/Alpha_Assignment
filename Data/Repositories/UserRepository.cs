using Data.Entities;
using Data.Interfaces;
using Microsoft.Data.SqlClient;

namespace Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString = @"Data Source=localhost;Initial Catalog=sql_database_alpha;Persist Security Info=True;User ID=Emeli;Password=***********;Trust Server Certificate=True";

    public UserRepository()
    {
        CreateUsersTableIfNotExists();
    }
    public void CreateUsersTableIfNotExists()
    {
        try
        {
            string createUsersTableQuery = @"

        IF OBJECT_ID('Users', 'U') IS NULL
            CREATE TABLE Users (
            Id INT IDENTITY(1,1) PRIMARY KEY,
            FirstName NAVCHAR(50) NOT NULL,
            LastName NAVCHAR(50) NOT NULL,
            Email NAVCHAR(100) NOT NULL,
            PhoneNumber NAVCHAR(15) NULL
         );

        ";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(createUsersTableQuery, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
           
        }
    }
    public bool Create(UserEntity userEntity)
    {
        try
        {
            string insertQuery = @"
                INSERT INTO Users (FirstName, LastName, Email, PhoneNumber)
                VALUES (@FirstName, @LastName, @Email, @PhoneNumber)
            ";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@FirstName", UserEntity.FirstName);
            command.Parameters.AddWithValue("@LastName", UserEntity.LastName);
            command.Parameters.AddWithValue("@Email", UserEntity.Email);
            command.Parameters.AddWithValue("@PhoneNumber", UserEntity.PhoneNumber ?? (object)DBNull.Value);

            command.ExecuteNonQuery();

        }

        catch (Exception ex)
        {
            return false;
        }
    }
     public IEnumerable<UserEntity> GetAll()
    {
        try
        {
            var users = new List<UserEntity>();

            string selectQuery = @"SELECT * FROM Users";
            
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(selectQuery, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new UserEntity()
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                });
            }

            return users;
        }

        catch (Exception ex)
        {
            return null!;
        }
    }

}
