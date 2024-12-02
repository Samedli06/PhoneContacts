using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneContacts.Model
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ContactNumber { get; set; }
        public string website { get; set; }
        public string UserId { get; set; }

        public Contact( string name , string surname , string contactnumber)
        {
            this.Name = name;
            this.Surname = surname;
            this.ContactNumber = contactnumber;
        }
    }
}
