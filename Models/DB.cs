using System.Data.SqlClient;
using Dapper;

namespace mi_web_personal.Models{
public class DB
{
   private static string _connectionString {get;set;} = @"Server=A-PHZ2-CIDI-16;DataBase=IsTravel;Trusted_Connection=true;";


    public static List<Usuario> Seleccionar(string sql){
        List<Usuario> listaUsuario = new List<Usuario>();
        using(SqlConnection db = new SqlConnection(_connectionString)){
            listaUsuario = db.Query<Usuario>(sql).ToList();
        }
        return listaUsuario;
    }


    public static void CrearUsuario(Usuario objeto){
            using(SqlConnection db = new SqlConnection(_connectionString)){
            string sql = $"INSERT INTO Usuario(Nombre,Nick,Contrasena,Mail) VALUES('{objeto.Nombre}','{objeto.Nick}','{objeto.GetContrasena()}','{objeto.GetMail()}')";
            db.Execute(sql, new{Nombre = objeto.Nombre, Nick = objeto.Nick, Contrasena = objeto.GetContrasena(), Mail=objeto.GetMail()});
        }
        }
        

    
}
}