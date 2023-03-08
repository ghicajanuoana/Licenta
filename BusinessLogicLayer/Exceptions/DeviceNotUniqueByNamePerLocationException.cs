namespace BusinessLogicLayer.Exceptions
{
    public class DeviceNotUniqueByNamePerLocationException : Exception
    {
        public DeviceNotUniqueByNamePerLocationException(string message) : base(message) { }
    }
}
