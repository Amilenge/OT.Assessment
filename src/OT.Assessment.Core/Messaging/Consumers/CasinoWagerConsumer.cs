using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Data.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OT.Assessment.Core.Messaging.Consumers
{
    public class CasinoWagerConsumer : DefaultBasicConsumer, IConsumer
    {
        public string QueueName { get; } = Constants.CasinoWagerQueueName;

        private readonly IMapper _mapper;
        private readonly IConnection _rabbitMqConnection;

        protected readonly IDbContextFactory<OtAssessmentDbContext> _contextFactory;

        public CasinoWagerConsumer(
           IConnection connection,
           IDbContextFactory<OtAssessmentDbContext> contextFactory,
           IMapper mapper
           )
        {
            _mapper = mapper;
            _rabbitMqConnection = connection;
            _contextFactory = contextFactory;
        }

        public override void HandleBasicDeliver(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> rBody)
        {
            Console.WriteLine($" {nameof(CasinoWagerRequest)} event received.");

            var body = rBody.ToArray();
            var rawMessage = Encoding.UTF8.GetString(body);
            var wagerRequest = JsonSerializer.Deserialize<CasinoWagerRequest>(rawMessage);

            using var _dbContext = _contextFactory.CreateDbContext();

            try
            {
                using (var dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    // Idempotency check - Ensure the wager is not processed multiple times
                    if (_dbContext.Wagers.AsNoTracking().Any(w => w.WagerId == wagerRequest.WagerId))
                    {
                        Console.WriteLine("Wager already processed.");
                        _dbContext.Database.CommitTransaction();
                        return;
                    }

                    var account = _dbContext.Accounts.FirstOrDefault(x => x.AccountId == wagerRequest.AccountId);

                    if (account == null)
                    {
                        account = _mapper.Map<Account>(wagerRequest);
                        _dbContext.Add(account);
                    }
                    else
                    {
                        account.TotalAmountSpend += wagerRequest.Amount;
                        _dbContext.Accounts.Update(account);
                    }

                    // Handle Wager and Transaction
                    var wager = _mapper.Map<Wager>(wagerRequest);
                    _dbContext.Add(wager);

                    var transaction = _mapper.Map<Transaction>(wagerRequest);
                    _dbContext.Add(transaction);

                    // Save all changes
                    _dbContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing wager: {ex.Message}");
                throw;
            }

            // message are auto Ack on successful exit
        }

    }
}
