using MySql.Data.MySqlClient;
using provaProgetto.Models;
namespace provaProgetto.Dipendenze
{
    public interface IGestioneDati
    {
        public List<Appuntamento> futureBookings(int userId);
        public List<Appuntamento> pastBookings(int userId);
        public List<Appuntamento> ListaAppuntamenti(int userId);
        public Appuntamento? GetAppuntamento(int id);
        public bool InserisciAppuntamentoUser(int idEvento, Utente u);
        public bool DeleteAppuntamento(int id);
        public bool DeleteAllBookings(int id);
        public List<Evento> ListaEventi(int id);
        public Evento? GetEvento(int id);
        public bool DeleteEvento(int id);
        public bool UpdateEvento(Evento e);
        public bool InserisciEvento(Evento e);
        public List<Utente> ListPartecipants(int id);
    }
}
