namespace HordeEngine
{
    // Data that will be updated at the beginning of every rendering frame.
    public class TimeManager
    {
        public float Time;
        public float DeltaTime;
        public float SlowableTime;
        public float DeltaSlowableTime;
        public float SlowingFactor = 1.0f;

        public void UpdateTime(float delta)
        {
            DeltaTime = delta;
            Time += DeltaTime;

            DeltaSlowableTime = DeltaTime * SlowingFactor;
            SlowableTime += DeltaSlowableTime;
        }
    }
}
