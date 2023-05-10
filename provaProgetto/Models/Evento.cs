using System;
namespace provaProgetto.Models
{
	public class Evento
	{
        public int id { get; set; }
        public string nome { get; set; }
        public string materia { get; set; }
        public DateTime data { get; set; }
		public int idOrganizzatore { get; set; }
        public int numPosti { get; set; }
        public int durata { get; set; }
        public int nPartecipanti { get; set; }
	}


}

