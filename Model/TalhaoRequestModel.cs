namespace APIAgroCoreOrquestradora.Model
{
    public class TalhaoRequestModel
    {
        public int TalhaoId { get; set; }
        public int PropriedadeId { get; set; }
        public string Nome { get; set; }
        public double Area { get; set; }
        public string Descricao { get; set; }
    }
}
