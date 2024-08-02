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
                    Sesion.SetearSesion(DB.Seleccionar($"select * from Usuario where mail='{mail}'")[0]);
                    ViewBag.estaLogeado=Sesion.EstaLogeado;
                    ViewBag.usuario=DB.Seleccionar($"select * from Usuario where mail='{mail}'")[0];
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
    [HttpPost]
    public IActionResult ActualizarFotoPerfil(IFormFile archivo){
        if(archivo.Length>0){
            string wwwRootLocal=this.Environment.ContentRootPath+@"\wwwroot\fotosPerfil\"+archivo.FileName;
            Sesion.userActual.FotoPerfil=@"fotosPerfil\"+archivo.FileName;
            using(var stream=System.IO.File.Create(wwwRootLocal)){
                archivo.CopyToAsync(stream);
            }
        }
        return View("index");
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
    public IActionResult setearFotoPerfil(){
        return View();
    }
    private string FormatearError(string error)
    {
        
        return "<div class='alert alert-danger alert-dismissible' role='alert'><div>"+ error + "</div>   <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
    }
}
