namespace WebApi
{
    public class CommonStrings
    {
        //null
        public string NullDeviceParameters = "Device parameters should not be null !"; 
        public string NullLocationParameters = "Location parameters should not be null !";
        public string NullDevice = "Null device. Please provide a valid device !";
        public string NullMaintenance = "Null maintenance. Please provide a valid maintenance !";
        public string NullDeviceReading = "Device Reading is null !";
        public string NullDeviceReadingType = "Null device reading type. Please provide a valid device reading type !";
        public string NullDeviceType = "Null device type. Please provide a valid device type !";
        public string NullLocation = "Null location. Please provide a valid location !";
        public string NullThreshold = "Null threshold. Please provide a valid threshold !";
        public string NullUser = "User is null !";

        //not found
        public string NotFoundDevice = "Device not found!";
        public string NotFoundDeviceReadingType = "Device reading type not found!";
        public string NotFoundDeviceType = "Device type not found!";
        public string NotFoundLocation = "Location not found !";
        public string NotFoundThreshold = "Threshold not found!";
        public string NotFoundUser = "User not found !";
        public string NotFoundMaintenance = "Maintenance not found !";

        //is used
        public string UsedDevice = "This device is used. Please add another device!";
        public string UsedDeviceReadingType = "The device reading type is used!";

        //invalid name
        public string InvalidName = "Invalid name. Please provide a valid name!";

        //invalid id
        public string InvalidId = "Invalid id. Please provide a valid id!";
        public string InvalidIdDeviceType = "Invalid device type id !";

        //unique name
        public string EnterUniqueName = "Please enter an unique name!";
        public string UniqueNameCombination = "Please enter an unique device type and device reading type combination!";

        //remove success
        public string RemovedDevice = "Device removed successfully";
        public string RemovedDeviceReadingType = "Device reading type removed successfully";
        public string RemovedDeviceType = "Device type removed successfully";
        public string RemovedLocation = "Location removed successfully";
        public string RemovedThreshold = "Threshold removed successfully";
        public string RemovedUser = "User removed successfully";
        public string RemovedMaintenance = "Maintenance removed successfully";
    }
}
