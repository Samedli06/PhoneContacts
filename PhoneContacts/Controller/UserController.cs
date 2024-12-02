using PhoneContacts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PhoneContacts.Model;
using System;
using System.Collections.Generic;
using ContacsDLL;

namespace PhoneContacts.Controller
{
    public class UserController
    {
        string connectionString = "Data Source=LAPTOP-SJO69TLI;Initial Catalog=PhoneContact;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";
        public List<User> users = new List<User>();

        public UserController()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM users", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User(
                                        reader["email"].ToString(),
                                        reader["password"].ToString()
                                    );

                                users.Add(user);
                            }
                        }
                    }
                }
            }

            catch(Exception ex)  
            {
                Console.WriteLine($"Error {ex.Message}");
            }
        }

        public void SignUP(User user)
        {
            try
            {
                ValidationManager.ValidateDuplicateUser(users.Select(c => new UserDll(c.email, c.password)).ToList() ,user.email , user.password);
                ValidationManager.passwordController(user.password);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO users (email, password) VALUES (@email, @password)";

                    using (SqlCommand command = new SqlCommand(query , connection))
                    {
                        command.Parameters.AddWithValue("@email", user.email);
                        command.Parameters.AddWithValue("@password", user.password);

                        int rowsaffected = command.ExecuteNonQuery();

                        if (rowsaffected > 0) 
                        {
                            Console.WriteLine("user added successfully");
                            users.Add(user);
                        }

                        else
                        {
                            Console.WriteLine("failed to add user");
                        }
                    }
                }
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
        }

        public User SignIn(string email , string password)
        {
            foreach (User item in users)
            {
                if(item.email == email && item.password == password)
                {
                    
                    return item;
                }
            }
            return null;

        }
    }
}