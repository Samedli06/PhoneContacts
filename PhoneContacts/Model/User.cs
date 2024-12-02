using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneContacts.Model
{
    public class User
    {
       
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string email { get; set; }
        public string  password { get; set; }

        public User(string email , string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
