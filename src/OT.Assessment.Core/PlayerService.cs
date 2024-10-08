using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Api.Contract.Response;
using OT.Assessment.Core.Dtos;
using OT.Assessment.Core.Messaging.Senders;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Core
{
    public class PlayerService : IPlayerService
    {
        private readonly IMessageSender<CasinoWagerRequest> _casinoWagerMessageSender;
        private readonly OtAssessmentDbContext _databaseContext;
        private readonly IMapper _mapper;

        public PlayerService(
            IMessageSender<CasinoWagerRequest> casinoWagerMessageSender,
            OtAssessmentDbContext databaseContext,
            IMapper mapper
            )
        {
            _casinoWagerMessageSender = casinoWagerMessageSender;
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task AddCasinoWager(CasinoWagerRequest casinoWager)
        {
            await _casinoWagerMessageSender.SendAsync(casinoWager);
        }

        public async Task<CasinoWagerResponse<WagerDto>> RetrieveCasinoWagers(Guid accountId, int? pageSize, int? page)
        {
            var newPageSize = pageSize ?? 10;
            var newPage = page ?? 1;

            var wagerResult = await _databaseContext.Wagers
                .Where(x => x.AccountId == accountId)
                .Skip((newPage - 1) * newPageSize)
                .Take(newPageSize)
                .Include(w => w.Transactions)
                .ToListAsync();

            if(wagerResult.Count < 1)
            {
                return new CasinoWagerResponse<WagerDto>();
            }

            var totalWagers = await _databaseContext.Wagers
                .Where(x => x.AccountId == accountId)
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalWagers / newPageSize);

            return new CasinoWagerResponse<WagerDto>
            {
                data = _mapper.Map<List<WagerDto>>(wagerResult) ?? new List<WagerDto>(),
                page = newPage,
                pageSize = newPageSize,
                total = totalWagers,
                totalPages = (int)Math.Ceiling((double)totalWagers / newPageSize)
            };
        }

        public async Task<List<AccountDto>> RetrieveTopSpender(int? count)
        {
            count ??= 1;
            var topSpenders = await _databaseContext.Accounts
                   .OrderByDescending(a => a.TotalAmountSpend)
                   .Take((int)count)
                   .ToListAsync();

            return _mapper.Map<List<AccountDto>>(topSpenders) ?? new List<AccountDto>();
        }
    }
}
