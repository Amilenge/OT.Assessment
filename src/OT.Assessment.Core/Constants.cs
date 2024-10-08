using OT.Assessment.Api.Contract.Request;

namespace OT.Assessment.Core
{
    public static class Constants
    {
        public static string CasinoWagerQueueName = typeof(CasinoWagerRequest).FullName;

        public static string WagerRoutingKey = CasinoWagerQueueName;
    }
}