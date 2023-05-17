using System;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;
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

        public IEnumerable<Appuntamento> ListaAppuntamenti(int userId)
        {
            using var con = new MySqlConnection(s);
            var query ="SELECT * from appuntamenti WHERE idUtente = @userId";
            var param = new
            {
                userId = userId
            };
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
            var query = "SELECT * from appuntamenti where idUtente=@userId";
            var param = new { userId = id };
            return con.Query<Appuntamento>(query, param).ToList();

        }
        public bool DeleteAppuntamentiUser(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "DELETE from appuntamenti WHERE idUtente=@userId";
            var param = new { userId = id };
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

        public List<Appuntamento> futureBookings(int userId)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT appuntamenti.id, appuntamenti.idEvento,appuntamenti.idUtente,appuntamenti.dataPrenotazione FROM appuntamenti " +
                "INNER JOIN eventi on appuntamenti.idEvento=eventi.id WHERE data>='@today' AND utenti.id=@userId";
            var param = new
            {
                today = DateTime.Now,
                userId = userId
            };
            return con.Query<Appuntamento>(query, param).ToList();
        }

        public List<Appuntamento> pastBookings(int userId)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT appuntamenti.id, appuntamenti.idEvento,appuntamenti.idUtente,appuntamenti.dataPrenotazione FROM appuntamenti " +
                "INNER JOIN eventi on appuntamenti.idEvento=eventi.id WHERE data>='@today' AND utenti.id=@userId";
            var param = new
            {
                today = DateTime.Now,
                userId = userId
            };
            return con.Query<Appuntamento>(query).ToList();
        }

        //CRUD eventi
        public List<Evento> ListaEventi(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT * from eventi WHERE idOrganizzatore = @userId";
            var param = new
            {
                userId = id
            };
            return con.Query<Evento>(query, param).ToList();
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
            var query = "DELETE from eventi WHERE id=@idEvento";
            var param = new { idEvento = id };
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

        public List<Evento> ListaEventiUser(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT eventi.* " +
                "FROM eventi inner join appuntamenti on eventi.id = appuntamenti.idEvento " +
                "WHERE idUtente=@userId";
            var param = new
            {
                userId = id
            };
            return con.Query<Evento>(query, param).ToList();
        }

        public List<Utente> ListPartecipants(int id)
        {
            using var con = new MySqlConnection(s);
            var query = "SELECT utenti.* FROM utenti " +
                "INNER JOIN appuntamenti on utenti.id = appuntamenti.idUtente where idEvento =@idEvento";
            var param = new { idEvento = id };
            return con.Query<Utente>(query, param).ToList();
        }

    }
}

