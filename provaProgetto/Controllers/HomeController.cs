using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using provaProgetto.Models;

namespace provaProgetto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    private GestioneDati g;
    private GestioneUtente gUtente;
    private Autenticazione gestioneAutenticazione;
    //IHttpContextAccessor httpContextAccessor
    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        //string idSessione = httpContextAccessor.HttpContext.Session.Id;
        g = new GestioneDati(_configuration);
        gUtente = new GestioneUtente(_configuration);

        //gestioneAutenticazione = new Autenticazione(httpContextAccessor.HttpContext.Session);

        //httpContextAccessor.HttpContext.Items["username"] = gestioneAutenticazione.GetUsername();

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View(new Utente());
    }

    [HttpPost]
    public IActionResult Login(Utente user)
    {

        Utente u = gUtente.FindUtente(user.mail, user.password);
        if (u != null)
        {
            gestioneAutenticazione.ImpostaUtente(user.mail);
            return RedirectToAction("log", "Index");
        }
        else
        {
            ModelState.AddModelError("", "Utente sconosciuto");
            return View(u);
        }
    }

    public IActionResult SignIn()
    {
        return View(new Registrazione());
    }

    [HttpPost]
    public IActionResult SignIn(Registrazione registrazione)
    {
        if(!ModelState.IsValid)
        {
            return View(registrazione);
        }
        bool esito = gUtente.InserisciUtente(registrazione);
        if(esito)
        {
            return Ok(registrazione);
        }
        else
        {
            ModelState.AddModelError("", "Errore di inserimento");
            return View(registrazione);
        }
    }

    public IActionResult Logout(ISession s)
    {
        s.Clear();
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

