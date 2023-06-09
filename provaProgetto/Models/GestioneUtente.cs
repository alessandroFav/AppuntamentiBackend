﻿using System;
using System.Text;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Dapper;
using BC = BCrypt.Net.BCrypt;
using provaProgetto.Dipendenze;
//using provaProgetto.Controllers;

namespace provaProgetto.Models
{
	public class GestioneUtente: IGestioneUtente
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
                psw = BC.HashPassword(u.password)

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
            string query = "UPDATE utenti SET nome=@name, cognome=@surname, mail=@email, VerifiedAt=@verifiedat WHERE id=@Id";
            var param = new
            {
                Id = user.id,
                name=user.nome,
                surname=user.cognome,
                email=user.mail,
                //psw = BC.HashPassword(user.password),
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
        public bool ResetPassword(int userId, string newPassword)
        {
            using var con = new MySqlConnection(s);
            string query = "UPDATE utenti SET password=@psw WHERE id=@userId";
            var param = new
            {
                userId = userId,
                psw = BC.HashPassword(newPassword)
            };
            try
            {
                con.Execute(query, param);
                return true;
            }
            catch { return false; }
        }

    }
}

