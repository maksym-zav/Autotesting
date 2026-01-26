using System.Diagnostics;

namespace WebApps.Extensions
{
    public class WaitResult<T>
    {
        public T Result { get; set; }
        public Exception Exception { get; set; }
        public bool Success { get; set; } = true;
    }

    internal static class StopwatchExtension
    {
        private const int defaultRefreshDelay = 250;
        public static WaitResult<T> Wait<T>(this Stopwatch timer, Func<T> method, TimeSpan timeout)
        {
            WaitResult<T> result = new WaitResult<T>();
            T funcResult;
            do
            {
                try
                {
                    funcResult = method();
                    if (funcResult != null)
                    {
                        result.Result = funcResult;
                        return result;
                    }
                }
                catch (Exception exception)
                {
                    result.Exception = exception;
                }

                Thread.Sleep(defaultRefreshDelay);
            }
            while (timer.Elapsed < timeout);
            result.Success = false;
            return result;
        }

    }
}