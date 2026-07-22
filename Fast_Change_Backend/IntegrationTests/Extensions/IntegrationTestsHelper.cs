namespace IntegrationTests.Extensions;

public static class IntegrationTestsHelper
{
    public static async Task WaitAsync(Func<Task<bool>> predicate, int timeoutMs = 10000, int pollMs = 100)
    {
        using var cts = new CancellationTokenSource(timeoutMs);

        while (!cts.IsCancellationRequested)
        {
            if (await predicate()) return;

            await Task.Delay(pollMs, cts.Token);
        }

        throw new TimeoutException("Condition was not satisfied.");
    }
}
