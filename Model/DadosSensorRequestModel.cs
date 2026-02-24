namespace APIAgroCoreOrquestradora.Model
{
    public class DadosSensorRequestModel
    {
        public int TalhaoId { get; set; }
        public double UmidadeSolo { get; set; }
        public double Temperatura { get; set; }
        public double NivelPreciptacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}
