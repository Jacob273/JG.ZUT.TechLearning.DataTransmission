namespace JG.TechLearning.DataTransmission
{
    /// <summary>
    /// Probka sygnalu (wartosc sygnalu w danej chwili).
    /// </summary>
    public struct Sample
    {
        public Sample(double value, double deltaTime)
        {
            Value = value;
            DeltaTime = deltaTime;
        }

        public double Value { get; set; }
        public double DeltaTime { get; set; }
    }
}
