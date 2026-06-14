namespace IoTSensors
{
    public class ProductionCounterSensor
    {
        private readonly Random _random = new();

        public int Read()
        {
            return _random.Next(1, 6);
        }
    }
}