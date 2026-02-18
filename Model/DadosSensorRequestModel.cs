namespace APIAgroCoreOrquestradora.Model
{
    public class DadosSensorRequestModel
    {
        public int DadosSensorId { get; set; }
        public int TalhaoId { get; set; }
        public double UmidadeSolo { get; set; }
        public int Temperatura { get; set; }
        public double NivelPreciptacao { get; set; }
    }
}
