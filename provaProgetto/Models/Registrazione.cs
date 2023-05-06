using System;
using System.ComponentModel.DataAnnotations;

namespace provaProgetto.Controllers
{
	public class Registrazione
	{
        [Required]
        public string nome { get; set; }
        [Required]
        public string cognome { get; set; }
        [Required]
        public string mail { get; set; }
        [Required]
        public string password { get; set; }
        [Compare(nameof(password))]
        public string confermaPassword { get; set; }
        [Required]
        public string ruolo { get; set; }

	}
}

