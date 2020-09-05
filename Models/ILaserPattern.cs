namespace Interfaces
{
    /// <summary>
    /// Used to set animation speed of a pattern, value can be used for a stopwatch
    /// </summary>
    public enum AnimationSpeed
    {
        Off = 2,
        Slow = 5,
        Medium = 7,
        Fast = 10,
        VeryFast = 13
    }

    public interface ILaserPattern
    {
        public void Project(int total);
        public AnimationSpeed GetAnimationSpeed();
    }
}