using System.Buffers;
using System.Text;
using System.Text.Json;

namespace OT.Assessment.Core.Messaging
{
    public static class MessageSerializer
    {
        private static readonly ArrayPool<byte> _bytePool = ArrayPool<byte>.Shared;

        public static byte[] SerializeMessage(object message)
        {
            var jsonString = JsonSerializer.Serialize(message);

            var byteCount = Encoding.UTF8.GetByteCount(jsonString);
            var buffer = _bytePool.Rent(byteCount);

            try
            {
                // Convert the JSON string to UTF8 bytes and fill the buffer
                var actualByteCount = Encoding.UTF8.GetBytes(jsonString, 0, jsonString.Length, buffer, 0);

                // Create an exact-sized array to return
                var result = new byte[actualByteCount];
                Array.Copy(buffer, result, actualByteCount);

                return result;
            }
            finally
            {
                _bytePool.Return(buffer);
            }
        }
    }

}
