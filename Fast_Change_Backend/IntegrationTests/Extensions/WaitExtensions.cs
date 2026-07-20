namespace IntegrationTests.Extensions;

public static class WaitExtensions
{
    public static async Task WaitUntilAsync(
        Func<Task<bool>> condition,
        TimeSpan timeout,
        TimeSpan? interval = null)
    {
        var delay = interval ?? TimeSpan.FromMilliseconds(200);
        var start = DateTime.UtcNow;

        while (DateTime.UtcNow - start < timeout)
        {
            if (await condition()) return;
            await Task.Delay(delay);
        }
        
        throw new TimeoutException("The expected condition was not met within the timeout period.");
    }
}
