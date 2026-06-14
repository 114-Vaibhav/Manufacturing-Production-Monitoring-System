namespace IoTSensors
{
    public class TemperatureSensor
    {
        private readonly Random _random = new();

        public double Read()
        {
            return Math.Round(30 + (_random.NextDouble() * 50), 2);
        }
    }
}