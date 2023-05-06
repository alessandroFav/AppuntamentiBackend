using System;
namespace provaProgetto.Models
{
	public class Appuntamento
	{
		public int id { get; set; }
        public int idEvento { get; set; }
        public int idUtente { get; set; }
        public DateTime dataPrenotazione { get; set; }
        
	}
}

