using System;
using Interfaces;

namespace Models
{
    public class SerialCommand
    {
        public string SaveSettings(LaserSettings settings)
        {
            return $"(set-settings,{Newtonsoft.Json.JsonConvert.SerializeObject(settings)}";
        }

        public string GetSettings()
        {
            return "(get-settings)";
        }

        public string SetAnimationSpeed(AnimationSpeed animationSpeed)
        {
            Console.WriteLine((int)animationSpeed);
            return $"(set-animationspeed,{(int)animationSpeed})";
        }
    }
}