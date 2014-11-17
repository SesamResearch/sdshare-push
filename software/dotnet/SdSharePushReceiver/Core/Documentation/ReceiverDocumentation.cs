namespace SdShare.Documentation
{
    public class ReceiverDocumentation
    {
        public string Graph { get; set; }
        public string Type { get; set; }
        public bool Idempotent { get; set; }
        public string IdempotencyCacheExpiration { get; set; }
        public string IdempotencyCacheMethod { get; set; }
    }
}
