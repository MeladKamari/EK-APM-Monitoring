using Elastic.Apm.Api;

namespace Api.BackGroundService;

public class SendSMSScheduler : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var transaction =
                Elastic.Apm.Agent.Tracer.StartTransaction("Kafka", "Kafka");
            try
            {
                await transaction.CaptureSpan("Publish to topic", ApiConstants.ActionExec, async (span) =>
                {
                    span.Duration = 5000;
                    span.Outcome = Outcome.Success;
                }, "Kafka", ApiConstants.SubTypeGrpc, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            transaction.End();

            //SendSMS
            await Task.Delay(10000, stoppingToken);
        }
    }
}