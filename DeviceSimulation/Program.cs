using DeviceSimulation;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlTypes;
using System.Net.Http.Json;
using System.Timers;

class Program
{
    static void Main(string[] args)
    {
        var constants= new Constants();
        var connectionToReadingValues = ConfigurationManager.GetSection("ReadingTypes") as NameValueCollection;
        var connectionToValueRange = ConfigurationManager.GetSection("ValuesRange") as NameValueCollection;
        int seconds = Convert.ToInt32(ConfigurationManager.AppSettings[constants.secondsKey]);
        int deviceId = Convert.ToInt32(ConfigurationManager.AppSettings[constants.deviceIdKey]);
        string apiUrl = ConfigurationManager.AppSettings[constants.apiUrlKey];
        string sendValuesUrl = ConfigurationManager.AppSettings[constants.sendValueApi];
        var timersList = new List<System.Timers.Timer>();
        Random rnd = new Random();
        var keylist = connectionToReadingValues.AllKeys;

        SetTimers(connectionToReadingValues, ref timersList, deviceId, keylist,connectionToValueRange, sendValuesUrl);

        while (true)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var heartbeat = new Heartbeat { DeviceId = deviceId, LastPingedTs = DateTime.UtcNow };
                    client.BaseAddress = new Uri(apiUrl);
                    var response = client.PutAsJsonAsync(apiUrl, heartbeat).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Ping sent!");
                    }
                    else
                    {
                        Console.WriteLine("Ping failed");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ping failed!");
            }
            Thread.Sleep(seconds * 1000);
        }
    }

    private static void AddParameters(object? sender, ElapsedEventArgs e, DeviceReadings paramater, string sendValuesUrl, double minValues, double maxValues)
    {
        Random rnd = new Random();
        var valueRead = Math.Round(rnd.NextDouble() * (maxValues - minValues) + minValues, 2);
        paramater.ValueRead = valueRead;
        paramater.ReceivedTs= DateTime.Now;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sendValuesUrl);
                var response = client.PostAsJsonAsync(sendValuesUrl, paramater).Result;
                Console.WriteLine(paramater.DeviceReadingTypeName + " " + paramater.ValueRead);
                Console.WriteLine(response.ToString());

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured!");
        }
    }

    private static void SetTimers( NameValueCollection connectionToReadingValues, ref List<System.Timers.Timer> timersList,
        int deviceId, String[] keylist, NameValueCollection connectionToValueRange, string sendValuesUrl)
    {
        foreach (var readingValues in connectionToReadingValues)
        {
            var index = timersList.Count;
            var timer = new System.Timers.Timer();
            timer.Interval = Convert.ToInt32(connectionToReadingValues[index]) * 1000;
            timersList.Add(timer);
            timer.Start();
        }

        foreach (var timer in timersList)
        {
            var index_timer = timersList.IndexOf(timer);
            var parameter = new DeviceReadings { DeviceId = deviceId };
            parameter.DeviceReadingTypeName = keylist[index_timer];
            var valueRangeList = connectionToValueRange[index_timer].Split(' ').ToArray();
            try
            {
                var minValues = Convert.ToDouble(valueRangeList[0]);
                var maxValues = Convert.ToDouble(valueRangeList[1]);
                if (minValues > maxValues)
                {
                    (minValues, maxValues) = (maxValues, minValues);
                }
                timer.Elapsed += new ElapsedEventHandler((sender, e) =>
                    AddParameters(sender, e, parameter, sendValuesUrl, minValues, maxValues)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid value range!");
            }
        }
    }
}
