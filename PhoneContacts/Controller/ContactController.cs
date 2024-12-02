using Microsoft.Data.SqlClient;
using PhoneContacts.Model;
using System;
using System.Collections.Generic;

using ContacsDLL;

namespace PhoneContacts.Controller
{
    public class ContactController
    {
        public List<Contact> contacts = new List<Contact>();
        string connectionString = "Data Source=LAPTOP-SJO69TLI;Initial Catalog=PhoneContact;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";

        public ContactController()
        {
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT Name, Surname, ContactNumber FROM contact", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Contact contact = new Contact(
                                    reader["Name"].ToString(),
                                    reader["Surname"].ToString(),
                                    reader["ContactNumber"].ToString()
                                );
                                contacts.Add(contact);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public void ShowContacts()
        {
            foreach (Contact contact in contacts)
            {
                Console.WriteLine($"Name: {contact.Name}");
                Console.WriteLine($"Surname: {contact.Surname}");
                Console.WriteLine($"Phone Number: {contact.ContactNumber}");
                Console.WriteLine("=========================");
            }
        }

        public void AddContact(Contact contact)
        {
            try
            {
                ValidationManager.ValidateFields(contact.Name, contact.Surname, contact.ContactNumber);

                ValidationManager.ValidateContactNumber(contact.ContactNumber);

                ValidationManager.ValidateDuplicate(contacts.Select(c => new ContactDLL(c.Name, c.Surname, c.ContactNumber)).ToList(), contact.ContactNumber);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO contact (Name, Surname, ContactNumber) VALUES (@Name, @Surname, @ContactNumber)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", contact.Name);
                        command.Parameters.AddWithValue("@Surname", contact.Surname);
                        command.Parameters.AddWithValue("@ContactNumber", contact.ContactNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Contact added successfully!");
                            contacts.Add(contact);
                        }
                        else
                        {
                            Console.WriteLine("Failed to add contact.");
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
                Console.WriteLine($"Error adding contact: {ex.Message}");
            }
        }

        public void RemoveContact(Contact contact) 
        { 
            contacts.Remove (contact);
        }

        public void UpdateContact(string contactNumber, string newName, string newSurname, string newContactNumber)
        {
            try
            {
                ValidationManager.ValidateFields(newName, newSurname, newContactNumber);

                ValidationManager.ValidateContactNumber(newContactNumber);

                if (contactNumber != newContactNumber)
                {
                    ValidationManager.ValidateDuplicate(contacts.Select(c => new ContactDLL(c.Name, c.Surname, c.ContactNumber)).ToList(), newContactNumber);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE contact SET Name = @Name, Surname = @Surname, ContactNumber = @NewContactNumber WHERE ContactNumber = @ContactNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", newName);
                        command.Parameters.AddWithValue("@Surname", newSurname);
                        command.Parameters.AddWithValue("@NewContactNumber", newContactNumber);
                        command.Parameters.AddWithValue("@ContactNumber", contactNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Contact updated successfully!");

                            Contact contact = contacts.FirstOrDefault(c => c.ContactNumber == contactNumber);
                            if (contact != null)
                            {
                                contact.Name = newName;
                                contact.Surname = newSurname;
                                contact.ContactNumber = newContactNumber;
                            }
                        }
                        else
                        {
                            Console.WriteLine("No contact found with the specified contact number.");
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
                Console.WriteLine($"Error updating contact: {ex.Message}");
            }
        }

    }
}
