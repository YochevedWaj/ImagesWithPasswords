using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesWithPasswords.Data
{
    public class ImagesRepository
    {
        private string _connectionString;
        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddImage(Image image)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Images Values(@fileName, @password, @viewCount) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@fileName", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
            cmd.Parameters.AddWithValue("@viewCount", image.ViewCount);
            connection.Open();
            image.ID = (int)(decimal)cmd.ExecuteScalar();
        }

        public Image GetImage(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }
            return new Image
            {
                ID = (int)reader["ID"],
                FileName = (string)reader["FileName"],
                Password = (string)reader["Password"],
                ViewCount = (int)reader["ViewCount"]
            };
        }

        public string GetPasswrod(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Password FROM Images WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            return (string)cmd.ExecuteScalar();
        }
        public void IncrementImageViewCount(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Images SET ViewCount += 1 WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
