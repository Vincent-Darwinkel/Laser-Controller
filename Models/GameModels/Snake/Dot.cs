using System;
using Models.LaserPatterns;

namespace Models.GameModels.Snake
{
    internal class Dot
    {
        internal LaserLine LeftWallPosition { get; set; }
        internal LaserLine BottomWallPosition { get; set; }
        internal LaserLine RightWallPosition { get; set; }
        internal LaserLine TopWallPosition { get; set; }

        private const int DotSize = 75;
        private readonly LaserSettings _settings;
        private readonly LaserPatternHelper _laserPatternHelper;

        public Dot(LaserSettings settings, LaserPatternHelper laserPatternHelper)
        {
            _settings = settings;
            _laserPatternHelper = laserPatternHelper;

            PlaceDotAtRandomPosition();
        }

        internal void PlaceDotAtRandomPosition()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            int dotXCenter = random.Next(_settings.maxLeft + DotSize, _settings.maxRight - DotSize);
            int dotYCenter = random.Next(_settings.minHeight + DotSize, _settings.maxHeight - DotSize);

            BottomWallPosition = new LaserLine
            {
                FirstPosition = new LaserPositionAndColors
                {
                    X = dotXCenter - DotSize,
                    Y = dotYCenter - DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                },

                SecondPosition = new LaserPositionAndColors
                {
                    X = dotXCenter + DotSize,
                    Y = dotYCenter - DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                }
            };

            RightWallPosition = new LaserLine
            {
                FirstPosition = new LaserPositionAndColors
                {
                    X = dotXCenter + DotSize,
                    Y = dotYCenter - DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                },

                SecondPosition = new LaserPositionAndColors
                {
                    X = dotXCenter + DotSize,
                    Y = dotYCenter + DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                }
            };

            TopWallPosition = new LaserLine
            {
                FirstPosition = new LaserPositionAndColors
                {
                    X = dotXCenter + DotSize,
                    Y = dotYCenter + DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                },

                SecondPosition = new LaserPositionAndColors
                {
                    X = dotXCenter - DotSize,
                    Y = dotYCenter + DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                }
            };

            LeftWallPosition = new LaserLine
            {
                FirstPosition = new LaserPositionAndColors
                {
                    X = dotXCenter - DotSize,
                    Y = dotYCenter + DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                },

                SecondPosition = new LaserPositionAndColors
                {
                    X = dotXCenter - DotSize,
                    Y = dotYCenter - DotSize,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                }
            };
        }
    }
}