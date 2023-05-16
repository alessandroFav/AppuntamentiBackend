using provaProgetto.Models;

namespace provaProgetto.Dipendenze
{
    public interface IJwtManager
    {
        public string GenerateJwtToken(Utente user);
        public int? ValidateToken(string token);
    }
}
