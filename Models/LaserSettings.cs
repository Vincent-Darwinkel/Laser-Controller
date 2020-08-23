namespace Models
{
    public class LaserSettings
    {
        public int maxLeft { get; set; }
        public int maxRight { get; set; }
        public int maxHeight { get; set; }
        public int minHeight { get; set; }
        public int[] maxLaserPower { get; set; }
    }
}