namespace WebApps
{
    /// <summary>
    /// This class represents wait parameters for different operations
    /// </summary>
    public class WaitOptions
    {
        public bool Throw { get; set; }
        public TimeSpan Timeout { get; set; }

        public static WaitOptions DefaultTimeThrow => new WaitOptions(TimeSpan.FromSeconds(1), true);
        public static WaitOptions DefaultTimeDoNotThrow => new WaitOptions(TimeSpan.FromSeconds(1));
        public static WaitOptions NullTimeDoNotThrow => new WaitOptions(TimeSpan.Zero);
        public static WaitOptions NullTimeThrow => new WaitOptions(TimeSpan.Zero, true);

        public WaitOptions(TimeSpan timeout, bool throwError = false)
        {
            Throw = throwError;
            Timeout = timeout;
        }
    }
}
