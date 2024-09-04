namespace Api.Entities;


public class SensorData
{
    public string SensorId { get; set; }  // شناسه یکتا برای سنسور
    public string SensorType { get; set; }  // نوع سنسور (مثلاً دما، رطوبت، فشار)
    public string Location { get; set; }  // محل نصب سنسور یا مکان جمع‌آوری داده
    public string Status { get; set; }  // وضعیت سنسور (مثلاً فعال، غیر فعال)
    public byte? BatteryLevel { get; set; }  // سطح باتری سنسور (در صورت وجود)
    public DateTime Timestamp { get; set; }  // زمان جمع‌آوری داده
    public float? Temperature { get; set; }  // داده دمای اندازه‌گیری شده (در صورت وجود)
    public float? Humidity { get; set; }  // داده رطوبت اندازه‌گیری شده (در صورت وجود)
    public float? Pressure { get; set; }  // داده فشار اندازه‌گیری شده (در صورت وجود)
    public string AdditionalData { get; set; }  // داده‌های اضافی که ممکن است متغیر باشند (به صورت JSON)

    public long Id { get; set; }  // شناسه یکتا برای هر رکورد (Primary Key)
    public DateTime CreatedAt { get; set; }  // زمان ایجاد رکورد
    public DateTime UpdatedAt { get; set; }  // زمان آخرین بروزرسانی رکورد

}


public class SensorDataGenerator
{
    private static Random _random = new Random();

    public static SensorData GenerateRandomSensorData()
    {
        SensorData sensorData = new SensorData
        {
            Id = _random.Next(1, 1000000),
            SensorId = "SENSOR-" + _random.Next(1, 100).ToString("D4"),
            SensorType = GetRandomSensorType(),
            Location = GetRandomLocation(),
            Status = GetRandomStatus(),
            BatteryLevel = (byte?)_random.Next(0, 101), // 0 to 100%
            Timestamp = DateTime.UtcNow,
            Temperature = GetRandomTemperature(),
            Humidity = GetRandomHumidity(),
            Pressure = GetRandomPressure(),
            AdditionalData = GetRandomAdditionalData(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return sensorData;
    }

    private static string GetRandomSensorType()
    {
        string[] sensorTypes = { "Temperature", "Humidity", "Pressure" };
        return sensorTypes[_random.Next(sensorTypes.Length)];
    }
    private static string GetRandomLocation()
    {
        string[] locations = { "Warehouse 1", "Warehouse 2", "Office 1", "Office 2" };
        return locations[_random.Next(locations.Length)];
    }

    private static string GetRandomStatus()
    {
        string[] statuses = { "Active", "Inactive", "Maintenance" };
        return statuses[_random.Next(statuses.Length)];
    }

    private static float? GetRandomTemperature()
    {
        return (float)Math.Round(_random.NextDouble() * 40 - 10, 2); 
    }

    private static float? GetRandomHumidity()
    {
        return (float)Math.Round(_random.NextDouble() * 100, 2); 
    }

    private static float? GetRandomPressure()
    {
        return (float)Math.Round(_random.NextDouble() * 50 + 950, 2);
    }

    private static string GetRandomAdditionalData()
    {
        return "{\"key1\":\"value1\", \"key2\":\"value2\"}"; 
    }
}
