using System.ComponentModel;

namespace Laboratorium_3.Models
{
    public class Contact
    {
        [DisplayName("Identyfikator")]
        public int Id { get; set; }

        [DisplayName("Imię")]
        public string Name { get; set; }

        [DisplayName("Nazwisko")]
        public string Surname { get; set; }

        [DisplayName("Adres e-mail")]
        public string Email { get; set; }

        [DisplayName("Miasto")]
        public string City { get; set; }

        [DisplayName("Numer telefonu")]
        public string PhoneNumber { get; set; }
    }
}
