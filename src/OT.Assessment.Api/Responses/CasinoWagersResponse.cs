using OT.Assessment.Core.Dtos;

namespace OT.Assessment.Api.Responses
{
    public class CasinoWagersResponse
    {
        public List<WagerDto> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }
}
