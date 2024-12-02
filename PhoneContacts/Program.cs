using System;
using System.Collections.Generic;
using System.Linq;
using ContacsDLL;
using PhoneContacts.Model;
using PhoneContacts.Controller;

namespace PhoneContacts
{
    class Program
    {
        static void Main(string[] args)
        {
            UserController userController = new UserController();
            ContactController contactController = new ContactController();

            bool isUserLoggedIn = false;
            User currentUser = null;

            while (!isUserLoggedIn)
            {
                Console.WriteLine("Welcome to Phone Contacts!");
                Console.WriteLine("1) Login");
                Console.WriteLine("2) Register");
                Console.Write("Please select an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("Enter your email: ");
                        string email = Console.ReadLine();
                        Console.WriteLine("Enter your password: ");
                        string password = Console.ReadLine();

                        currentUser = userController.SignIn(email, password);
                        if (currentUser != null)
                        {
                            isUserLoggedIn = true;
                            Console.WriteLine("Login successful!");
                        }
                        else
                        {
                            Console.WriteLine("Invalid email or password.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter your email: ");
                        email = Console.ReadLine();
                        Console.WriteLine("Enter your password: ");
                        password = Console.ReadLine();

                        currentUser = new User(email, password);
                        userController.SignUP(currentUser);
                        break;

                    default:
                        Console.WriteLine("Invalid option, please select 1 or 2.");
                        break;
                }
            }

            bool showMenu = true;
            while (showMenu)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1) Show All Contacts");
                Console.WriteLine("2) Add Contact");
                Console.WriteLine("3) Remove Contact");
                Console.WriteLine("4) Update Contact");
                Console.WriteLine("5) Logout");
                Console.Write("Please select an option: ");
                string menuOption = Console.ReadLine();

                switch (menuOption)
                {
                    case "1":
                        contactController.ShowContacts();
                        break;

                    case "2":
                        Console.WriteLine("Enter Contact Name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter Contact Surname: ");
                        string surname = Console.ReadLine();
                        Console.WriteLine("Enter Contact Number: ");
                        string contactNumber = Console.ReadLine();

                        Contact contact = new Contact(name, surname, contactNumber);
                        contactController.AddContact(contact);
                        break;

                    case "3":
                        Console.WriteLine("Enter Contact Number to remove: ");
                        string contactToRemove = Console.ReadLine();
                        Contact contactToDelete = contactController.contacts.FirstOrDefault(c => c.ContactNumber == contactToRemove);
                        if (contactToDelete != null)
                        {
                            contactController.RemoveContact(contactToDelete);
                            Console.WriteLine("Contact removed.");
                        }
                        else
                        {
                            Console.WriteLine("Contact not found.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Enter Contact Number to update: ");
                        string contactNumberToUpdate = Console.ReadLine();
                        Console.WriteLine("Enter new Contact Name: ");
                        string newName = Console.ReadLine();
                        Console.WriteLine("Enter new Contact Surname: ");
                        string newSurname = Console.ReadLine();
                        Console.WriteLine("Enter new Contact Number: ");
                        string newContactNumber = Console.ReadLine();

                        contactController.UpdateContact(contactNumberToUpdate, newName, newSurname, newContactNumber);
                        break;

                    case "5":
                        showMenu = false;
                        Console.WriteLine("You have logged out successfully.");
                        break;

                    default:
                        Console.WriteLine("Invalid option, please select a valid menu option.");
                        break;
                }
            }
        }
    }
}
