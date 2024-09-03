namespace krolCakes.Models
{
    public class accesoModel
    {
        public int? correlativo { get; set; }
        public int? id_rol { get; set; }
        public int? id_modulo { get; set; }
    }

    public class accesoModelCompleto
    {
        public int? correlativo { get; set; }   //proviene del modelo acceso
        public int? id_rol { get; set; }        //proviene del modelo acceso
        public int? id_modulo { get; set; }     //proviene del modelo acceso
        public string? rol { get; set; }        //nombre de rol 
        public string? modulo { get; set; }     //nombre de modulo
        public string? url { get; set; }        //proviene del modelo modulo
        public string? icono { get; set; }      //proviene del modelo modulo

    }  
    
}
