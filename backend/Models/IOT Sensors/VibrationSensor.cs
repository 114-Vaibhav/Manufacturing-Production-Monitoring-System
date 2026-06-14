namespace IoTSensors
{
    public class VibrationSensor
    {
        private readonly Random _random = new();

        public double Read()
        {
            return Math.Round(_random.NextDouble() * 10, 2);
        }
    }
}