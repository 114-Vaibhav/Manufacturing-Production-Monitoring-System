namespace IoTSensors
{
    public class PowerSensor
    {
        private readonly Random _random = new();

        public double Read()
        {
            return Math.Round(100 + (_random.NextDouble() * 400), 2);
        }
    }
}