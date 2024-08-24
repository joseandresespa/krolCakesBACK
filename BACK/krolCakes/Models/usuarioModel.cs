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
}
