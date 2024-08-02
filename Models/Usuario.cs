namespace mi_web_personal.Models{

    public class Usuario{
        private int idUsuario{get;set;}
        public string Nombre {get;set;}
        public string Nick {get;set;}
        private string Contrasena {get;set;}
        public string Mail {set; private get;}
        public string FotoPerfil {get;set;}


        public Usuario(string nombre, string nick,string contrasena, string mail){
            this.Nombre=nombre;
            this.Nick=nick;
            this.Contrasena=contrasena;
            this.Mail=mail;
        }
        public Usuario(){
            
        }
        public bool CheckContra (string contrasenaPosible){
            if (contrasenaPosible==this.Contrasena){
                return true;
            }
            return false;
        }
        public string GetContrasena (){
            return Contrasena;
        }
        public string GetMail(){
            return Mail;
        }

    }
}