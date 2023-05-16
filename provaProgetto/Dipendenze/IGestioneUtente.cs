using provaProgetto.Models;

namespace provaProgetto.Dipendenze
{
    public interface IGestioneUtente
    {
        public Utente? FindUtente(int id);
        public Utente? FindUtente(string mail);
        public Utente? InserisciUtente(Registrazione u); 
        public bool UpdateUtente(Utente user);

    }
}
