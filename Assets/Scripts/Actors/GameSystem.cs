public abstract class GameSystem
{
    /// <summary>
    /// Lower number = higher priority
    /// </summary>
    public int Priority { get; set; }
    public abstract void Tick();
}
