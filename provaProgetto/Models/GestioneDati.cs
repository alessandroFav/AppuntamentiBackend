using System;
using System.Configuration;
using Dapper;
using MySql.Data.MySqlClient;
using provaProgetto.Models;

namespace provaProgetto.Controllers
{
	public class GestioneDati
	{
		private string s;
        public GestioneDati(IConfiguration configuration)
		{
            s = configuration.GetConnectionString("progettoConnection")!;
        }

		//CRUD appuntamenti
		public bool InserisciAppuntamento(Appuntamento a)
		{
            using var con = new MySqlConnection(s);
            var query = @"INSERT INTO appuntamenti(idEvento, idUtente, dataPrenotazione) VALUES(@idEv,@idUser,@data)";
            var param = new
            {
                idEv = a.idEvento,
                idUser = a.idUtente,
                data = a.dataPrenotazione,
            };
            bool esito;
            try
            {
                con.Execute(query, param);
                esito = true;
            }
            catch(Exception e)
            {
                esito = false;
            }
            return esito;
        }

        public Appuntamento GetAppuntamento(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT * from appuntamenti WHERE id="+id;
            IEnumerable<Appuntamento> app = con.Query<Appuntamento>(query);
            return app.ToList()[0];
        }

        public IEnumerable<Appuntamento> ListaAppuntamenti()
        {
            using var con = new MySqlConnection(s);
            var query ="SELECT * from appuntamenti";
            return con.Query<Appuntamento>(query);
        }


        //CRUD eventi
        public IEnumerable<Evento> ListaEventi()
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT * from eventi";
            return con.Query<Evento>(query);
        }

        public bool InserisciEvento(Evento e)
        {
            using var con = new MySqlConnection(s);
            var query = @"INSERT INTO eventi(nome,materia,data,idOrganizzatore,numPosti,durata,nPartecipanti) VALUES(@name,@mat,@date,@idOrg,@nPosti,@dur,@partecipanti)";
            var param = new
            {
                name = e.nome,
                mat = e.materia,
                date = e.data,
                idOrg = e.idOrganizzatore,
                nPosti = e.numPosti,
                dur = e.data,
                partecipanti=e.nPartecipanti
            };
            bool esito;
            try
            {
                con.Execute(query, param);
                esito = true;
            }
            catch (Exception err)
            {
                esito = false;
            }
            return esito;
        }

        public bool UpdateEvento(Evento e)
        {
            using var con = new MySqlConnection(s);
            var query = "UPDATE eventi SET nome=@name,materia=@mat,data=@date,idOrganizzatore=@idOrd,numPosti=@nP,durata=@dur,nPartecipanti=@part"+
                "WHERE id=@idEvento";
            var param = new
            {
                idEvento=e.id,
                name = e.nome,
                mat = e.materia,
                date = e.data,
                idOrg = e.idOrganizzatore,
                nPosti = e.numPosti,
                dur = e.data,
                partecipanti = e.nPartecipanti

            };
            bool esito;
            try
            {
                con.Execute(query, param);
                esito = true;
            }
            catch (Exception err)
            {
                esito = false;
            }
            return esito;
        }



    }
}

