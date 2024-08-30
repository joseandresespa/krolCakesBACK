namespace krolCakes.Models
{
    public class pastelrealizadoModel
    {
        public int? id { get; set; }
        public int? id_tipo_evento { get; set; }
        public int? id_pedido { get; set; }
        public string? imagen { get; set; }
    }
    public class pastelrealizadoModelCompleto
    {
        public pastelrealizadoModel? pastelrealizado { get; set; }
        public tipoeventoModel tipoevento { get; set; }
        public pedidoModel pedido { get; set; }
    }
}
