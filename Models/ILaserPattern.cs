namespace Interfaces
{
    /// <summary>
    /// Used to set animation speed of a pattern, value can be used for a stopwatch
    /// </summary>
    public enum AnimationSpeed
    {
        Slow = 3000,
        Medium = 2000,
        Fast = 1000,
        VeryFast = 500
    }

    public interface ILaserPattern
    {
        void Project(AnimationSpeed animationSpeed);
        public AnimationSpeed GetAnimationSpeed();
    }
}