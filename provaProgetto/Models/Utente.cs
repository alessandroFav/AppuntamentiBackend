using System;
namespace provaProgetto.Controllers
{
	public class Utente
	{
		public int id { get; set; }
		public string nome { get; set; }
        public string cognome { get; set; }
		public string mail { get; set; }
        public string password { get; set; }
        public DateTime? VerifiedAt { get; set; }
	}
}

