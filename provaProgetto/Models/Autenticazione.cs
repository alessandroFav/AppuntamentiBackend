using System;
namespace provaProgetto.Controllers
{
	public class Autenticazione
	{
        public ISession session { get; set; }

        public Autenticazione(ISession session)
		{
			this.session = session;
		}

		public void ImpostaUtente(string username)
		{
			session.SetString("username", username);
		}

		public string GetUsername()
		{
			string username = "";
			if(session.GetString("username")!=null)
			{
				username = session.GetString("username")!;
			}
			return username;
		}
	}
}

