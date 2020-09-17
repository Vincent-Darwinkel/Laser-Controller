using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Enums;
using Interfaces;
using Models.LaserPatterns;

namespace Models.GameModels.Snake
{
    public class LaserSnake : IGame
    {
        private readonly Laser _laser;
        private readonly Snake _snake;
        private readonly LaserSettings _settings;
        private readonly LaserPatternHelper _laserPatternHelper;

        private Dot _dot;
        private bool _gameStarted;
        private const int SnakeIncreaseValue = 25;
        private PlayerDirection _playerDirection = PlayerDirection.Right;
        private int _timerInterval = 50;
        private int _previousGameSpeedIncrease;
        private int _iterations;
        private Timer _timer;

        public LaserSnake(Laser laser, LaserSettings settings, LaserPatternHelper laserPatternHelper)
        {
            _snake = new Snake(settings, laserPatternHelper, SnakeIncreaseValue);
            _settings = settings;
            _laser = laser;
            _dot = new Dot(settings, laserPatternHelper);
            _laserPatternHelper = laserPatternHelper;

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _timer = new Timer(50);
            _timer.Elapsed += TimerTick;
            _timer.AutoReset = true;
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            MoveSnake();
            _timer.Interval = _timerInterval;
            _iterations++;

            if (_iterations - _previousGameSpeedIncrease > 50 && _timerInterval > 15)
            {
                _timerInterval--;
                _previousGameSpeedIncrease = _iterations;
            }

            if (SnakeTouchesWall() || SnakeTouchesHimself())
            {
                Stop();
                return;
            }

            if (!SnakeTouchesDot()) return;
            EatDot();
            IncreaseSnakeLength();
        }
        
        public int GetScore()
        {
            return _snake.DotsEaten;
        }

        public void Move(GameKey key)
        {
            if (key == GameKey.ArrowUp && _playerDirection != PlayerDirection.Down)
            {
                _playerDirection = PlayerDirection.Up;
            }

            else if (key == GameKey.ArrowDown && _playerDirection != PlayerDirection.Up)
            {
                _playerDirection = PlayerDirection.Down;
            }

            else if (key == GameKey.ArrowLeft && _playerDirection != PlayerDirection.Right)
            {
                _playerDirection = PlayerDirection.Left;
            }

            else if (key == GameKey.ArrowRight && _playerDirection != PlayerDirection.Left)
            {
                _playerDirection = PlayerDirection.Right;
            }
        }

        private bool SnakeTouchesDot()
        {
            LaserPositionAndColors snakeHeadPosition = _snake.SnakePositions.Last();
            return snakeHeadPosition.Y >= _dot.RightWallPosition.FirstPosition.Y &&
                   snakeHeadPosition.Y <= _dot.RightWallPosition.SecondPosition.Y && 
                   snakeHeadPosition.X >= _dot.BottomWallPosition.FirstPosition.X &&
                   snakeHeadPosition.X <= _dot.BottomWallPosition.SecondPosition.X;
        }

        private bool SnakeTouchesWall()
        {
            LaserPositionAndColors snakeHeadPosition = _snake.SnakePositions.Last();

            return snakeHeadPosition.X >= _settings.maxRight || snakeHeadPosition.X <= _settings.maxLeft ||
                   snakeHeadPosition.Y >= _settings.maxHeight || snakeHeadPosition.Y <= _settings.minHeight;
        }

        private bool SnakeTouchesHimself()
        {
            LaserPositionAndColors snakeHeadPosition = _snake.SnakePositions.Last();

            foreach (var snakeBodyPosition in _snake.SnakePositions.GetRange(0, _snake.SnakePositions.Count - 1))
            {
                if (snakeHeadPosition.Y == snakeBodyPosition.Y && snakeHeadPosition.X == snakeBodyPosition.X)
                {
                    return true;
                }
            }

            return false;
        }

        private void MoveSnake()
        {
            int x = _snake.SnakePositions.Last().X;
            int y = _snake.SnakePositions.Last().Y;

            if (_playerDirection == PlayerDirection.Up) y += SnakeIncreaseValue;
            if (_playerDirection == PlayerDirection.Down) y -= SnakeIncreaseValue;
            if (_playerDirection == PlayerDirection.Left) x -= SnakeIncreaseValue;
            if (_playerDirection == PlayerDirection.Right) x += SnakeIncreaseValue;

            _snake.SnakePositions.Add(new LaserPositionAndColors
            {
                X = x,
                Y = y,
                LaserColors = _laserPatternHelper.GetRandomLaserColors()
            });

            _snake.SnakePositions.RemoveAt(0);
        }

        private void DrawSnake()
        {
            for (int i = 0; i < _snake.SnakePositions.Count; i++)
            {
                LaserPositionAndColors snakePositionAndColors = _snake.SnakePositions[i];

                _laser.SendTo(snakePositionAndColors.X, snakePositionAndColors.Y);
                if (i == 0) System.Threading.Thread.SpinWait(25000);
                _laser.On(snakePositionAndColors.LaserColors);
            }

            _laser.Off();
        }

        private void DrawDot()
        {
            _laser.SendTo(_dot.BottomWallPosition.FirstPosition.X, _dot.BottomWallPosition.FirstPosition.Y);
            System.Threading.Thread.SpinWait(20000);
            _laser.On(_dot.BottomWallPosition.FirstPosition.LaserColors);

            _laser.SendTo(_dot.BottomWallPosition.SecondPosition.X, _dot.BottomWallPosition.SecondPosition.Y);
            System.Threading.Thread.SpinWait(20000);

            _laser.SendTo(_dot.RightWallPosition.SecondPosition.X, _dot.RightWallPosition.SecondPosition.Y);
            System.Threading.Thread.SpinWait(20000);
            _laser.On(_dot.RightWallPosition.SecondPosition.LaserColors);

            _laser.SendTo(_dot.TopWallPosition.SecondPosition.X, _dot.TopWallPosition.SecondPosition.Y);
            System.Threading.Thread.SpinWait(20000);
            _laser.On(_dot.TopWallPosition.SecondPosition.LaserColors);

            _laser.SendTo(_dot.LeftWallPosition.SecondPosition.X, _dot.LeftWallPosition.SecondPosition.Y);
            System.Threading.Thread.SpinWait(20000);
            _laser.On(_dot.LeftWallPosition.SecondPosition.LaserColors);

            _laser.Off();
        }

        private void DrawWall()
        {
            _laser.SendTo(_settings.maxLeft, _settings.minHeight);
            System.Threading.Thread.SpinWait(25000);
            _laser.On(_laserPatternHelper.GetRandomLaserColors());

            _laser.SendTo(_settings.maxRight, _settings.minHeight);
            System.Threading.Thread.SpinWait(25000);

            _laser.SendTo(_settings.maxRight, _settings.maxHeight);
            System.Threading.Thread.SpinWait(25000);
            _laser.On(_laserPatternHelper.GetRandomLaserColors());

            _laser.SendTo(_settings.maxLeft, _settings.maxHeight);
            System.Threading.Thread.SpinWait(25000);
            _laser.On(_laserPatternHelper.GetRandomLaserColors());

            _laser.SendTo(_settings.maxLeft, _settings.minHeight);
            System.Threading.Thread.SpinWait(25000);
            _laser.On(_laserPatternHelper.GetRandomLaserColors());

            _laser.Off();
        }

        private void PlaceDot()
        {
            _dot.PlaceDotAtRandomPosition();
        }

        private void IncreaseSnakeLength()
        {
            int x = _snake.SnakePositions.Last().X;
            int y = _snake.SnakePositions.Last().Y;
            var newSnakePositions = new List<LaserPositionAndColors>();

            for (int i = 0; i < 5; i++)
            {
                newSnakePositions.Add(new LaserPositionAndColors
                {
                    X = x,
                    Y = y,
                    LaserColors = _laserPatternHelper.GetRandomLaserColors()
                });
            }
            
            newSnakePositions.AddRange(_snake.SnakePositions);
            _snake.SnakePositions = newSnakePositions;
        }

        private void EatDot()
        {
            _snake.DotsEaten++;
            PlaceDot();
        }

        public void Start()
        {
            _gameStarted = true;
            _timer.Enabled = true;
            _timer.Start();
            PlaceDot();

            while (_gameStarted)
            {
                DrawSnake();
                DrawDot();
                DrawWall();
            }

            _timer.Enabled = false;
            _timer.Stop();
        }

        public void Stop()
        {
            Console.WriteLine(_snake.DotsEaten);
            _gameStarted = false;
            _timerInterval = 50;
            _timer.Interval = _timerInterval;
            _snake.ResetSnake();
            _timer.Enabled = false;
            _timer.Stop();
            _playerDirection = PlayerDirection.Right;
        }

        public void Restart()
        {
            _snake.ResetSnake();
        }

    }
}