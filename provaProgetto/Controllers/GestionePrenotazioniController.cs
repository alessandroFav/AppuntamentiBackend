using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Tls;
using provaProgetto.Attributes;
using provaProgetto.Middlewares;
using provaProgetto.Models;

namespace provaProgetto.Controllers
{
    [Route("api")]
    [ApiController]
    //[Authorize]
    [provaProgetto.Attributes.Authorize]
    public class GestionePrenotazioniController : Controller
    {
        private readonly ILogger<GestionePrenotazioniController> _logger;
        private readonly IConfiguration _configuration;
        private HttpContext _context;

        private GestioneDati g;
        private GestioneUtente gUtenti;

        public GestionePrenotazioniController(ILogger<GestionePrenotazioniController> logger, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _context = httpContextAccessor.HttpContext!;
            g = new GestioneDati(_configuration);
            gUtenti = new GestioneUtente(_configuration);
        }

        //users

        [HttpGet("users")]
        public IActionResult GetUtente()
        {
            Utente? user = (Utente?)_context.Items["user"];
            if (user != null)
            {
                var ris = new
                {
                    name = user!.nome,
                    surname = user!.cognome,
                    email = user!.mail
                };
                return Ok(ris);
            }
            else { return NotFound("User not found"); }
        }

        [HttpPatch("users/resetPassword")]
        public IActionResult ResetPassword([FromBody] string newPassword)
        {
            Utente? user = (Utente?)_context.Items["user"];

            if (user!.VerifiedAt == null)
                return StatusCode(StatusCodes.Status401Unauthorized, "Email not verified");

            bool esito = gUtenti.ResetPassword(user!.id, newPassword);
            if (esito)
            {
                return Ok(esito);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, "Update error"); }
        }

        [HttpPut("users/update")]
        public IActionResult UpdateUtente([FromBody] UpdateUtente userData)
        {
            Utente? user = (Utente?)_context.Items["user"];
            if (user!.VerifiedAt == null)
                return StatusCode(StatusCodes.Status401Unauthorized, "Email non verificata");
            user.nome = String.IsNullOrEmpty(userData.nome) ? user.nome : userData.nome;
            user.cognome = String.IsNullOrEmpty(userData.cognome) ? user.cognome : userData.cognome;
            user.mail = String.IsNullOrEmpty(userData.email) ? user.mail : userData.email;

            bool esito = gUtenti.UpdateUtente(user!);
            if (esito)
            {
                return Ok(esito);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, "Update Errore"); }
        }


        //bookings

        [HttpGet("bookings/futureBookings")]
        public IActionResult FutureBookings()
        {
            Utente user = (Utente)_context.Items["user"]!;
            var listaApp = g.futureBookings(user.id);
            List<object> ris = new List<object>();
            foreach(Appuntamento app in listaApp)
            {
                Evento? evento = g.GetEvento(app.idEvento);
                if (evento == null)
                    return NotFound("Enevt not found");
                Utente owner = gUtenti.FindUtente(evento.idOrganizzatore)!;
                ris.Add(new
                {
                    id = app.id,
                    title = evento.nome,
                    start = evento.data,
                    end = evento.data.AddMinutes(evento.durata),
                    description = evento.materia,
                    organizer = new
                    {
                        name = owner.nome,
                        surname = owner.cognome,
                        email = owner.mail
                    }
                });
            }
            return Ok(ris);
        }

        [HttpGet("bookings/pastBookings")]
        public IActionResult PastBookings()
        {
            Utente user = (Utente)_context.Items["user"]!;
            var listaApp = g.pastBookings(user.id);
            List<object> ris = new List<object>();
            foreach (Appuntamento app in listaApp)
            {
                Evento? evento = g.GetEvento(app.idEvento);
                if (evento == null)
                    return NotFound("Event not found");
                Utente owner = gUtenti.FindUtente(app.idUtente)!;

                ris.Add(new
                {
                    id = app.id,
                    title = evento.nome,
                    start = evento.data,
                    end = evento.data.AddMinutes(evento.durata),
                    organizer = new
                    {
                        name = owner.nome,
                        surname = owner.cognome,
                        email = owner.mail
                    }
                });
            }

            return Ok(ris);
        }

        [HttpGet("bookings/{idAppuntamento}")]
        public IActionResult getAppuntamento(int idAppuntamento)
        {
            Appuntamento? app = g.GetAppuntamento(idAppuntamento);
            if (app != null)
            {
                Evento? evento = g.GetEvento(app.idEvento);
                if (evento == null)
                    return NotFound("Event not found");

                Utente owner = gUtenti.FindUtente(app.idUtente)!;
                return Ok(new
                {
                    id = app.id,
                    title = evento.nome,
                    start = evento.data,
                    end = evento.data.AddMinutes(evento.durata),
                    description = evento.materia,
                    organizer = new
                    {
                        name = owner.nome,
                        surname = owner.cognome,
                        email = owner.mail
                    }
                });
            }
            else { return NotFound("Booking not found"); }
        }

        [HttpGet("bookings/all")]
        public IActionResult ListaAppuntamenti()
        {
            Utente user = (Utente)_context.Items["user"]!;
            var listaApp = g.ListaAppuntamenti(user.id);
            List<object> ris = new List<object>();
            foreach (Appuntamento app in listaApp)
            {
                Evento? evento = g.GetEvento(app.idEvento);
                if (evento == null)
                    return NotFound("Event not found");
                Utente owner = gUtenti.FindUtente(app.idUtente)!;

                ris.Add(new
                {
                    id = app.id,
                    title = evento.nome,
                    start = evento.data,
                    end = evento.data.AddMinutes(evento.durata),
                    organizer = new
                    {
                        name = owner.nome,
                        surname = owner.cognome,
                        email = owner.mail
                    }
                });
            }

            return Ok(ris);
        }

        [HttpPost("bookings/{idEvento}")]
        public IActionResult InserisciAppuntamentoUser(int idEvento)
        {
            Utente user = (Utente)_context.Items["user"]!;
            bool esito = g.InserisciAppuntamentoUser(idEvento, user);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore inserimento");
            }
        }

        [HttpDelete("bookings/{idAppuntamento}")]
        public IActionResult DeleteAppuntamento(int idAppuntamento)
        {
            bool esito = g.DeleteAppuntamento(idAppuntamento);
            if (esito)
            {
                return Ok(true);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, "Delete error"); }
        }

        [HttpDelete("bookings")]
        public IActionResult DeleteBookings()
        {
            Utente user = (Utente)_context.Items["user"]!;
            bool esito = g.DeleteAllBookings(user.id);
            if (esito)
            {
                return Ok(true);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, "Delete errore"); }
        }


        //events

        [HttpGet("events")]
        public IActionResult ListaEventi()
        {
            Utente user = (Utente)_context.Items["user"]!;
            var listaEventi = g.ListaEventi(user.id);
            List<object> ris = new List<object>();
            foreach (Evento e in listaEventi)
            {
                ris.Add(new
                {
                    id = e.id,
                    title = e.nome,
                    start = e.data,
                    end = e.data.AddMinutes(e.durata),
                    description = e.materia,
                    nPartecipants = e.nPartecipanti,
                    maxPartecipants = e.numPosti
                });
            }

            return Ok(ris);
        }

        [HttpPost("events/{idEvento}")]
        public IActionResult getEvento(int idEvento)
        {
            Evento? evento = g.GetEvento(idEvento);
            if (evento != null)
            {
                return Ok(new
                {
                    id = evento.id,
                    title = evento.nome,
                    start = evento.data,
                    end = evento.data.AddMinutes(evento.durata),
                    description = evento.materia,
                    nPartecipants = evento.nPartecipanti,
                    maxPartecipants = evento.numPosti
                });
            }
            else { return NotFound("Event not found"); }
        }

        [HttpDelete("events/{idEvento}")]
        public IActionResult DeleteEvento(int idEvento)
        {
            bool esito = g.DeleteEvento(idEvento);
            if (esito)
            {
                return Ok(true);
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, "Delete error"); }
        }

        [HttpPut("events/{idEvento}")]
        public IActionResult UpdateEvento(int idEvento, [FromBody] UpdateEvento userData)
        {
            Evento? evento = g.GetEvento(idEvento);
            if (evento == null)
                return NotFound("Event not found");

            evento.nome = String.IsNullOrEmpty(userData.title) ? evento.nome : userData.title;
            evento.materia = String.IsNullOrEmpty(userData.description) ? evento.materia : userData.description;
            evento.data = userData.start == null ? evento.data : (DateTime)userData.start;
            evento.durata = userData.end == null ? evento.durata : Convert.ToInt32((userData.end! - userData.start).Value.TotalMinutes);
            evento.numPosti = userData.maxParticipants == null ? evento.numPosti : (int)userData.maxParticipants;

            bool esito = g.UpdateEvento(evento);
            if (esito)
            {
                return Ok(new
                {
                    id = evento.id,
                    title = evento.nome,
                    start = evento.data,
                    end = evento.data.AddMinutes(evento.durata),
                    description = evento.materia,
                    nPartecipants = evento.nPartecipanti,
                    maxPartecipants = evento.numPosti
                });
            }
            else { return StatusCode(StatusCodes.Status500InternalServerError, "Update Errore"); }
        }

        [HttpPost("events")]
        public IActionResult InserisciEvento([FromBody] UpdateEvento userData)
        {
            Utente user = (Utente)_context.Items["user"]!;

            Evento evento = new Evento()
            {
                nome = userData.title!,
                materia = userData.description!,
                data = (DateTime)userData.start!,
                durata = Convert.ToInt32((userData.end! - userData.start!).Value.TotalMinutes),
                idOrganizzatore = user.id,
                numPosti = (int)userData.maxParticipants!,
                nPartecipanti = 0
            };
            bool esito = g.InserisciEvento(evento);
            if (esito)
            {
                return Ok(esito);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore inserimento");
            }
        }


        //participants

        [HttpGet("participants/{idEvento}")]
        public IActionResult getParticipantsEvent(int idEvento)
        {
            var participants = g.ListPartecipants(idEvento);
            List<object> ris = new List<object>();
            foreach (Utente user in participants)
            {
                ris.Add(new
                {
                    id = user.id,
                    name = user.nome,
                    surname = user.cognome,
                    email = user.mail
                });
            }

            return Ok(ris);

        }
        


    }
}

