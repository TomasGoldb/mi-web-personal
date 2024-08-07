using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mi_web_personal.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace mi_web_personal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IWebHostEnvironment Environment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        Environment=environment;
    }

    public IActionResult Index()
    {
        ViewBag.estaLogeado=Sesion.EstaLogeado;
        ViewBag.usuario=Sesion.userActual;
        if(setearFotoPerfil!=null){
        ViewBag.fotoPerfil=Sesion.userActual.FotoPerfil;
        }else{
            ViewBag.fotoPerfil=@"\fotosPerfil\perfilVacio.png";
        }
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
                Sesion.SetearSesion(user);
                return RedirectToAction("index");
            } else{
                ViewBag.error=FormatearError("ERROR_001_YaExisteNickoMail");
                return View("register");
            }
        } else{
            ViewBag.error=FormatearError("ERROR_002_ContraNoCoincide");
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
                    Sesion.SetearSesion(DB.Seleccionar($"select * from Usuario where mail='{mail}'")[0]);
                    ViewBag.estaLogeado=Sesion.EstaLogeado;
                    ViewBag.usuario=DB.Seleccionar($"select * from Usuario where mail='{mail}'")[0];
                    return RedirectToAction("index");
                }else{
                    ViewBag.error=FormatearError("ERROR_003_ContraIncorrecta");
                    return View("login");
                }
            } else{
                ViewBag.error=FormatearError("ERROR_005_MailIncorrecto");
                return View("login");
            }
    }

    public IActionResult ActualizarFotoPerfil(IFormFile archivo){
        bool seCambio=Sesion.userActual.CambiarFoto(archivo, Environment);
        if(seCambio){
            
            return RedirectToAction("index");
        }else{
            ViewBag.error=FormatearError("ERROR_004_SinArchivo");
            return View("setearfotoperfil");
        }
        
    }
    public IActionResult login()
    {
        return View();
    }
    public IActionResult logout(){
        Sesion.LogOut();
        return View("Index");
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
    public IActionResult setearFotoPerfil(){
        return View();
    }
    private string FormatearError(string error)
    {
        string mensaje=error;
        if(error=="ERROR_001_YaExisteNickoMail"){
            mensaje="Ya existe un usuario con ese nick o mail.";
        }else if(error=="ERROR_002_ContraNoCoincide"){
            mensaje="Las contraseñas no coinciden.";
        }else if(error=="ERROR_003_ContraIncorrecta"){
            mensaje="La contraseña es incorrecta.";
        }else if(error=="ERROR_004_SinArchivo"){
            mensaje="No has ingresado ningún archivo!";
        } else if(error=="ERROR_005_MailIncorrecto"){
            mensaje="El mail ingresado es incorrecto.";
        }
        return "<div class='alert alert-danger alert-dismissible' role='alert'><div>"+ mensaje + "</div>   <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
    }
}
