using System;
using System.Collections.Generic;
using Models.LaserPatterns;

namespace Models.GameModels.Snake
{
    internal class Snake
    {
        internal List<LaserPositionAndColors> SnakePositions { get; set; }
        internal int DotsEaten { get; set; }

        private readonly LaserSettings _settings;
        private readonly LaserPatternHelper _laserPatternHelper;
        private int _snakeIncreaseValue = 0;

        public Snake(LaserSettings settings, LaserPatternHelper laserPatternHelper, int snakeIncreaseValue)
        {
            _settings = settings;
            _laserPatternHelper = laserPatternHelper;
            _snakeIncreaseValue = snakeIncreaseValue;
            SetSnakeStartPosition();
        }

        private void SetSnakeStartPosition()
        {
            int xCenter = (_settings.maxLeft + _settings.maxRight) / 2;
            int yCenter = (_settings.minHeight + _settings.maxHeight) / 2;

            int maxSnakeWidth = Math.Abs(_settings.maxLeft) + Math.Abs(_settings.maxRight);
            int snakeLength = maxSnakeWidth / 5;

            var defaultSnakePosition = new List<LaserPositionAndColors>();

            for (int x = xCenter - snakeLength / 2; x < snakeLength / 2; x += _snakeIncreaseValue)
            {
                defaultSnakePosition.Add(new LaserPositionAndColors
                {
                    X = x,
                    Y = yCenter,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                });
            }

            SnakePositions = defaultSnakePosition;
        }

        public void ResetSnake()
        {
            SetSnakeStartPosition();
            DotsEaten = 0;
        }
    }
}