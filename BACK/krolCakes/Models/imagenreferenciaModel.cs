namespace krolCakes.Models
{
    public class imagenreferenciaModel
    {
        public int? id { get; set; }
        public int? id_pedido { get; set; }
        public string? imagen { get; set; }
        public string? observaciones { get; set; }
    }
    public class imagenreferenciaModelCompleto
    {
        public imagenreferenciaModel? imagenreferencia { get; set; }
        public pedidoModel? pedido { get; set; }
    }
}
