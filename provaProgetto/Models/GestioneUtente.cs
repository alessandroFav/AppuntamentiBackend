using System;
using System.Text;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Dapper;

namespace provaProgetto.Controllers
{
	public class GestioneUtente
	{
		private string s; 
		public GestioneUtente(IConfiguration configuration)
		{
            s = configuration.GetConnectionString("progettoConnection")!;
        }
		public Utente? FindUtente(int id)
		{
			using var con = new MySqlConnection(s);
			string query = "SELECT * FROM utenti WHERE id=@userId";
            var param = new
            {
                userId = id
            };
            return con.Query<Utente>(query, param).SingleOrDefault();
        }
        public Utente? FindUtente(string mail)
        {
            using var con = new MySqlConnection(s);
            string query = "SELECT * FROM utenti WHERE mail=@email";
            var param = new
            {
                email = mail
            };
            return con.Query<Utente>(query, param).SingleOrDefault();
        }

        public Utente? InserisciUtente(Registrazione u)
        {
            using var con = new MySqlConnection(s);
            var query = "INSERT INTO utenti(nome,cognome,mail,password) VALUES(@name,@surname,@email,@psw)";
            var param = new
            {
                name = u.nome,
                surname = u.cognome,
                email = u.mail,
                psw = ComputeSha256Hash(u.password),

            };
            Utente? ris = null;
            try
            {
                con.Execute(query, param);
                ris = FindUtente(u.mail);
            }
            catch
            {}
            return ris;
        }
        public bool UpdateUtente(Utente user)
        {
            using var con = new MySqlConnection(s);
            string query = "UPDATE utenti SET nome=@name, cognome=@surname, mail=@email, password=@psw, VerifiedAt=@verifiedat WHERE id=@Id";
            var param = new
            {
                Id = user.id,
                name=user.nome,
                surname=user.cognome,
                email=user.mail,
                psw = ComputeSha256Hash(user.password),
                verifiedat = user.VerifiedAt,
            };
            try
            {
                con.Execute(query, param);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ComputeSha256Hash(string rawData)
        {
            //create SHA256
            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            //converto i byte in string
            StringBuilder builde = new StringBuilder();
            for (int i = 0; i < passwordBytes.Length; i++)
            {
                builde.Append(passwordBytes[i].ToString("x2"));
                //x = hesadecimal form
                //2 = 2 characteers

            }
            return builde.ToString();
        }

    }
}

