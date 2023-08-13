namespace test.Net_api;

[Serializable]
public class SpeedRecord
{
    public DateTime Timestamp { get; set; }
    public string VehicleNumber { get; set; }
    public double Speed { get; set; }
}
