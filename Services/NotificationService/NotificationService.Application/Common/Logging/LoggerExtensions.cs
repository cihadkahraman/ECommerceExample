using Microsoft.Extensions.Logging;
using NotificationService.Application.Common.Serialization;
using System.Text.Json;
using Serilog.Context;

namespace NotificationService.Application.Common.Logging
{
    public static class LoggerExtensions
    {
        public static void LogInformationWithPayload(
            this ILogger logger,
            string messageTemplate,
            params object[] contextItems)
        {
            LogWithPayload(logger, LogLevel.Information, null, messageTemplate, contextItems);
        }

        public static void LogErrorWithPayload(
            this ILogger logger,
            Exception exception,
            string messageTemplate,
            params object[] contextItems)
        {
            LogWithPayload(logger, LogLevel.Error, exception, messageTemplate, contextItems);
        }

        private static void LogWithPayload(
            ILogger logger,
            LogLevel level,
            Exception? exception,
            string messageTemplate,
            params object[] contextItems)
        {
            var disposables = new List<IDisposable>();

            for (int i = 0; i < contextItems.Length; i++)
            {
                var item = contextItems[i];

                string propertyName = item switch
                {
                    string str when str.ToLower().Contains("correlation") => "CorrelationId",
                    string str when str.ToLower().Contains("correlationid") => "CorrelationId",
                    _ when item?.GetType().Name.ToLower().Contains("command") == true => "Payload",
                    _ when item?.GetType().Name.ToLower().Contains("request") == true => "Payload",
                    _ when item?.GetType().Name.ToLower().Contains("response") == true => "Payload",
                    _ when item?.GetType().Name.ToLower().Contains("payload") == true => "Payload",
                    _ when item?.GetType().Name.ToLower().Contains("correlationid") == true => "CorrelationId",
                    _ => $"ContextItem{i + 1}"
                };

                var value = propertyName == "Payload"
                    ? JsonSerializer.Serialize(item, JsonDefaults.Options)
                    : item;

                disposables.Add(LogContext.PushProperty(propertyName, value!));
            }

            using (new DisposableCollection(disposables))
            {
                if (level == LogLevel.Information)
                    logger.LogInformation(messageTemplate);
                else if (level == LogLevel.Error)
                    logger.LogError(exception, messageTemplate);
            }
        }

        private class DisposableCollection : IDisposable
        {
            private readonly List<IDisposable> _disposables;
            public DisposableCollection(List<IDisposable> disposables) => _disposables = disposables;

            public void Dispose()
            {
                foreach (var d in _disposables)
                    d.Dispose();
            }
        }
    }
}
