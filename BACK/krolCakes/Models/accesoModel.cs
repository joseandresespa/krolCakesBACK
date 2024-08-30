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
        public accesoModel? acceso {  get; set; }
        public rolModel? rol { get; set; }
        public moduloModel? modulo { get; set; }

    }   

}
