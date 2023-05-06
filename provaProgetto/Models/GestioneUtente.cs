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
            s = configuration.GetConnectionString("ScuolaConnection")!;
        }
		public Utente FindUtente(string mail, string password)
		{
			using var con = new MySqlConnection(s);
			string query = @"SELECT * FROM utenti WHERE mail=@username and password=@hashPassword";
            var param = new
            {
                username = mail,
                hashPassword = ComputeSha256Hash(password)
            };
            return con.Query<Utente>(query, param).SingleOrDefault()!;
        }

        public bool InserisciUtente(Registrazione u)
        {
            using var con = new MySqlConnection(s);
            var query = @"INSERT INTO utenti(nome,cognome,mail,password,ruolo) VALUES(@name,@surname,@email,@psw,@role)";
            var param = new
            {
                name = u.nome,
                surname = u.cognome,
                email = u.mail,
                psw = ComputeSha256Hash(u.password),
                role = u.ruolo

            };
            bool esito;
            try
            {
                con.Execute(query, param);
                esito = true;
            }
            catch
            {
                esito = false;
            }
            return esito;
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

