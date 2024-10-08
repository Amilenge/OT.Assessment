using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Api.Contract.Response;
using OT.Assessment.Core.Dtos;

namespace OT.Assessment.Core
{
    public interface IPlayerService
    {
        Task AddCasinoWager(CasinoWagerRequest casinoWager);

        Task<CasinoWagerResponse<WagerDto>> RetrieveCasinoWagers(Guid accountId, int? pageSize, int? page);

        Task<List<AccountDto>> RetrieveTopSpender(int? count);
    }
}