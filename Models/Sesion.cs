namespace mi_web_personal.Models{

    public static class Sesion{
        public static bool EstaLogeado {get;set;}=false;
        public static Usuario userActual {get;set;}= new Usuario();


        public static void SetearSesion(Usuario use){
            userActual=use;
            EstaLogeado=true;
        }

        public static void LogOut(){
            EstaLogeado=false;
            userActual=new Usuario();
        }

    }
}