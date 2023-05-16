using System;
using System.Configuration;
using Dapper;
using MySql.Data.MySqlClient;
using provaProgetto.Dipendenze;
using provaProgetto.Models;

namespace provaProgetto.Models
{
	public class GestioneDati: IGestioneDati
	{
		private string s;
        public GestioneDati(IConfiguration configuration)
		{
            s = configuration.GetConnectionString("progettoConnection")!;
        }

		//CRUD appuntamenti

        public Appuntamento? GetAppuntamento(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT * from appuntamenti WHERE id=@idApp";
            var param = new
            {
                idApp = id
            };
            return con.Query<Appuntamento>(query, param).FirstOrDefault();
        }

        public IEnumerable<Appuntamento> ListaAppuntamenti()
        {
            using var con = new MySqlConnection(s);
            var query ="SELECT * from appuntamenti";
            return con.Query<Appuntamento>(query);
        }
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
            catch (Exception e)
            {
                esito = false;
            }
            return esito;
        }

        public bool InserisciAppuntamentoUser(int idEvento, Utente u)
        {
            using var con = new MySqlConnection(s);
            var query = @"INSERT INTO appuntamenti(idEvento, idUtente, dataPrenotazione) VALUES(@idEv,@idUser,@data)";
            var param = new
            {
                idEv = idEvento,
                idUser = u.id,
                data = DateTime.Now.ToString("yyyyMMdd") + "T" + DateTime.Now.ToString("HHmmss"),
            };
            //0001-01-01 00:00:00
            string q1 = "SELECT * from eventi WHERE id=" + idEvento;
            IEnumerable<Evento> e = con.Query<Evento>(q1);
            Evento considerato = e.ToList()[0];
            bool esito;
            if (considerato.nPartecipanti + 1 <= considerato.numPosti)
            {
                try
                {
                    con.Execute(query, param);
                    esito = true;
                }
                catch (Exception ex)
                {
                    esito = false;
                }
            }
            else
            {
                esito = false;
            }

            return esito;

        }

        public bool UpdateAppuntamento(int id, UpdateAppuntamento a)
        {
            using var con = new MySqlConnection(s);
            var query = "UPDATE appuntamenti SET @idEv,@idUser,@data" +
                "WHERE id=@id";
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
            catch (Exception err)
            {
                esito = false;
            }
            return esito;
        }

        public bool DeleteAppuntamento(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "DELETE from appuntamenti WHERE id=@idAppuntamento";
            var param = new { idAppuntamento = id };
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

        public List<Appuntamento> bookingsUser(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT * from appuntamenti where idUtente=" + id;
            IEnumerable<Appuntamento> app = con.Query<Appuntamento>(query);
            return app.ToList();

        }
        public bool DeleteAppuntamentiUser(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "DELETE from appuntamenti WHERE idUtente=" + id;
            bool esito;
            try
            {
                con.Execute(query);
                esito = true;
            }
            catch (Exception err)
            {
                esito = false;
            }
            return esito;
        }

        public List<Appuntamento> futureBookings()
        {
            using var con = new MySqlConnection(s);
            DateTime today = DateTime.Now;
            var query = "SELECT appuntamenti.id, appuntamenti.idEvento,appuntamenti.idUtente,appuntamenti.dataPrenotazione FROM appuntamenti " +
                "INNER JOIN eventi on appuntamenti.idEvento=eventi.id WHERE data>='" + today.ToString("yyyyMMdd") + "'";
            IEnumerable<Appuntamento> app = con.Query<Appuntamento>(query);
            return app.ToList();
        }

        public List<Appuntamento> pastBookings()
        {
            using var con = new MySqlConnection(s);
            DateTime today = DateTime.Now;
            var query = "SELECT appuntamenti.id, appuntamenti.idEvento,appuntamenti.idUtente,appuntamenti.dataPrenotazione FROM appuntamenti " +
                "INNER JOIN eventi on appuntamenti.idEvento=eventi.id WHERE data<'" + today.ToString("yyyyMMdd") + "'";
            IEnumerable<Appuntamento> app = con.Query<Appuntamento>(query);
            return app.ToList();

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
        public Evento? GetEvento(int id)
        {
            using var conn = new MySqlConnection(s);
            var query = "SELECT * from eventi WHERE id=@idEvento";
            var param = new
            {
                idEvento = id
            };
            return conn.Query<Evento>(query, param).FirstOrDefault();
        }

        public bool DeleteEvento(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "DELETE from eventi WHERE id=" + id;
            bool esito;
            try
            {
                con.Execute(query);
                esito = true;
            }
            catch (Exception err)
            {
                esito = false;
            }
            return esito;
        }

        public IEnumerable<Evento> ListaEventiUser(int idUser)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT eventi.id,eventi.nome,eventi.materia ,eventi.data,eventi.idOrganizzatore,eventi.numPosti,eventi.durata, eventi.nPartecipanti " +
                "FROM eventi inner join appuntamenti on eventi.id = appuntamenti.idEvento " +
                "WHERE idUtente=" + idUser;
            return con.Query<Evento>(query);
        }

        public IEnumerable<Evento> ListPartecipants(int idEvento)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT utenti.* FROM utenti " +
                "INNER JOIN appuntamenti on utenti.id = appuntamenti.idUtente where idEvento =" + idEvento;
            return con.Query<Evento>(query);
        }



    }
}

