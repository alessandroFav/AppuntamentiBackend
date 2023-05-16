using provaProgetto.Models;
namespace provaProgetto.Dipendenze
{
    public interface IGestioneDati
    {
        public bool InserisciAppuntamento(Appuntamento a);
        public Appuntamento? GetAppuntamento(int id);
        public IEnumerable<Appuntamento> ListaAppuntamenti();
        public IEnumerable<Evento> ListaEventi();
        public bool InserisciEvento(Evento e);
        public bool UpdateEvento(Evento e);
        public Evento? GetEvento(int id);
    }
}
