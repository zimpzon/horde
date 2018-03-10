namespace HordeEngine
{
    // Data that will be updated at the beginning of every rendering frame.
    public class TimeManager
    {
        public float Time;
        public float DeltaTime;
        public float SlowableTime;
        public float DeltaSlowableTime;
        public float TimeScale = 1.0f;

        public float GetDeltaTime(bool slowable)
        {
            return slowable ? SlowableTime : DeltaTime;
        }

        public void UpdateTime(float delta)
        {
            DeltaTime = delta;
            Time += DeltaTime;

            DeltaSlowableTime = DeltaTime * TimeScale;
            SlowableTime += DeltaSlowableTime;
        }
    }
}
