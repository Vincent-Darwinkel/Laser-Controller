using System;

namespace Models.LaserPatterns
{
    public class LaserPatternHelper
    {
        private readonly LaserSettings _settings;

        public LaserPatternHelper(LaserSettings settings)
        {
            _settings = settings;
        }

        public int GetRandomXPosition()
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(_settings.maxLeft, _settings.maxRight);
        }

        public int GetRandomYPosition()
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(_settings.minHeight, _settings.maxHeight);
        }

        public LaserColors GetRandomLaserColors()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            return new LaserColors
            {
                Red = _settings.maxLaserPower[0] > 85 ? new Random(Guid.NewGuid().GetHashCode()).Next(85, _settings.maxLaserPower[0]) : 0,
                Green = _settings.maxLaserPower[1] > 85 ? new Random(Guid.NewGuid().GetHashCode()).Next(85, _settings.maxLaserPower[1]) : 0,
                Blue = _settings.maxLaserPower[2] > 85 ? new Random(Guid.NewGuid().GetHashCode()).Next(85, _settings.maxLaserPower[2]) : 0
            };
        }
    }
}
