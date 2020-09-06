using Interfaces;

namespace Models
{
    public class SerialCommand
    {
        public string SaveSettings(LaserSettings settings)
        {
            return $"(set-settings,{Newtonsoft.Json.JsonConvert.SerializeObject(settings)})";
        }

        public string GetSettings()
        {
            return "(get-settings)";
        }

        public string SetAnimationSpeed(AnimationSpeed animationSpeed)
        {
            return $"(set-animationspeed,{(int)animationSpeed})";
        }

        public string Galvo(int x, int y)
        {
            return $"(g,{x}:{y})";
        }

        public string Lasers(LaserColors colors)
        {
            return $"(l,{colors.Red}:{colors.Green}|{colors.Blue})";
        }

        public string LasersOff()
        {
            return "(off)";
        }
    }
}