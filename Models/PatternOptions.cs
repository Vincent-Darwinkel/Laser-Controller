using Enums;

namespace Models
{
    public class PatternOptions
    {
        public string PatternName { get; set; }
        public AnimationSpeed AnimationSpeed { get; set; }
        public int DurationMilliseconds { get; set; }
        public int Total { get; set; }
    }
}