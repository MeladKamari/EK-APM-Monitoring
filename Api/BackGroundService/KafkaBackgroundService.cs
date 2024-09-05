using Elastic.Apm.Api;

namespace Api.BackGroundService;

public class KafkaBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var transaction =
                Elastic.Apm.Agent.Tracer.StartTransaction("Kafka", ApiConstants.TypeMessaging);
            try
            {
                await transaction.CaptureSpan("Publish to topic", "Kafka", async (span) =>
                {
                    //
                    //Data Push to Topic 
                    span.Duration = 5000;
                    span.Outcome = Outcome.Success;
                }, "Kafka", "Kafka", isExitSpan: true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            transaction.End();

            await Task.Delay(10000, stoppingToken);
        }
    }
}