using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mi_web_personal.Models;

namespace mi_web_personal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult register()
    {
        return View();
    }
    public IActionResult RegistrarUsuario(string nombre, string nick, string correo, string confirmaContra, string contra){
        bool coincide=false;
        if (contra==confirmaContra){
            Usuario user = new Usuario(nombre,nick,contra,correo);
            List<Usuario> usuarios=DB.Seleccionar("select * from Usuario");
            foreach(Usuario usu in usuarios){
                if(usu.Nick==user.Nick||usu.GetMail()==user.GetMail()){
                    coincide=true;
                } 
            }
            if (!coincide){
                ViewBag.error="";
                DB.CrearUsuario(user);
                return RedirectToAction("index");
            } else{
                ViewBag.error="ERROR_001_YaExiste";
                return View("register");
            }
        } else{
            ViewBag.error="ERROR_002_ContraNoCoincide";
            return View("register");
        }
        
    }
    public IActionResult LogearUsuario(string mail, string contra){
        bool coincide=false;
        List<Usuario> usuarios=DB.Seleccionar("select * from Usuario");
        foreach(Usuario usu in usuarios){
                if(usu.GetMail()==mail){
                    coincide=true;
                } 
            }
            if(coincide){
                if(contra==DB.Seleccionar($"select * from Usuario where mail='{mail}'")[0].GetContrasena()){
                    Sesion.EstaLogeado=true;
                    ViewBag.estaLogeado=Sesion.EstaLogeado;
                    return View("index");
                }else{
                    ViewBag.error=FormatearError("ERROR_003_ContraIncorrecta");
                    return View("login");
                }
            } else{
                ViewBag.error=FormatearError("ERROR_004_MailIncorrecto");
                return View("login");
            }
    }
    public IActionResult login()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    private string FormatearError(string error)
    {
        
        return "<div class='alert alert-danger alert-dismissible' role='alert'><div>"+ error + "</div>   <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
    }
}
