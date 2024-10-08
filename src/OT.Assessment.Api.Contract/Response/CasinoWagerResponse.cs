namespace OT.Assessment.Api.Contract.Response
{
    public class CasinoWagerResponse<T>
    {
        public List<T> data {  get; set; }
        public int page { get; set; }
        public int pageSize {  get; set; }
        public int total {  get; set; }
        public int totalPages { get; set; }
    }
}
