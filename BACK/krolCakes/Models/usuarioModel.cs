using krolCakes.Models;

namespace krolCakes.Models
{
    public class usuarioModel
    {
        public int? id { get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public string? contrasenia { get; set; }
        public bool? visibilidad { get; set; }
        public int? id_rol { get; set; }
    }
    public class usuarioModelCompleto
    {
        public int? id { get; set; }                 //proviene de modelo Usuario
        public string? nombre { get; set; }          //proviene de modelo Usuario
        public string? correo { get; set; }         //proviene de modelo Usuario
        public string? contrasenia { get; set; }    //proviene de modelo Usuario
        public bool? visibilidad { get; set; }      //proviene de modelo Usuario
        public int? id_rol { get; set; }            //proviene de modelo Usuario
        public string? rol { get; set; }            // nombre rol

    }
}