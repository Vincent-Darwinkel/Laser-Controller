using Enums;

namespace Models
{
    public class LaserAnimationStatus
    {
        public bool AnimationCanceled { get; set; } = false;
        public AnimationSpeed AnimationSpeed { get; set; } = AnimationSpeed.Off;
    }
}