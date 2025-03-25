using System.Text.Encodings.Web;
using System.Text.Json;

namespace NotificationService.Application.Common.Serialization
{
    public static class JsonDefaults
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}
