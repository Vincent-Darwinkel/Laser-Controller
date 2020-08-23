using System;
using Interfaces;

namespace Models
{
    public class SerialCommand
    {
        public string SaveSettings(Settings settings)
        {
            LaserSettings laserSettings = new LaserSettings
            {
                maxLeft = settings.MaxLeft,
                maxRight = settings.MaxRight,
                maxHeight = settings.MaxHeight,
                minHeight = settings.MinHeight,
                maxLaserPower = new[] {settings.RedPower, settings.GreenPower, settings.BluePower}
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(laserSettings);
        }

        public string SetAnimationSpeed(AnimationSpeed animationSpeed)
        {
            Console.WriteLine((int)animationSpeed);
            return $"(set-animationspeed,{(int)animationSpeed})";
        }
    }
}