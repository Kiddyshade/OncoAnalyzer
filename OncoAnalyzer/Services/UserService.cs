using System.Data.SqlClient;
using OncoAnalyzer.Models;

namespace OncoAnalyzer.Services
{
    public class UserService
    {
        private readonly IDbExecutor dbExecutor;

        public UserService(IDbExecutor dbExecutor)
        {
            this.dbExecutor = dbExecutor;
        }

        // Authenticate user by username and password
        public User Authenticate(string username,string password)
        {
            try
            {
                // SQL query to fetch user details by username
                string query = "SELECT * FROM Users WHERE Username = @Username";
                using (var reader = dbExecutor.ExecuteReader(query, command => 
                {
                    command.Parameters.Add(new SqlParameter("@Username", username));
                }))
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("Invalid username or password.");
                        return null;
                    }

                    // Get the stored password hash from the database
                    string storedHash = reader["PasswordHash"].ToString();

                    // Hash the input password
                    string hashedInputPassword = HashPassword(password);

                    //Console.WriteLine($"Stored Hash: {storedHash}");
                    //Console.WriteLine($"Input Hash: {hashedInputPassword}");


                    // compare hashes
                    if (storedHash != hashedInputPassword)
                    {
                        Console.WriteLine("Invalid username or password");
                        return null;
                    }

                    // Return authenticated user
                    return new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error during authentication: {ex.Message}");
                return null;
            }
        }

        // Hash password using HSA2_256 with UTF-16 encoding
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Convert the input password to a byte array using UTF-16 encoding
                byte[] bytes = System.Text.Encoding.Unicode.GetBytes(password);

                // Compute the SHA-256 hash
                byte[] hash = sha256.ComputeHash(bytes);

                // Return the Base64-encoded hash
                return Convert.ToBase64String(hash);
            }
        }
    }
}
