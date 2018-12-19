namespace FuelDistributorConsoleApplication
{
    public static class Logger
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
