using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Logic
{
    public class PatternLogic
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<ILaserPattern> _patterns = new List<ILaserPattern>();
        private Task _playPatternTask;

        public PatternLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            try
            {
                var patterns = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                    .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface);

                foreach (var pattern in patterns)
                    _patterns.Add((ILaserPattern)ActivatorUtilities.CreateInstance(_serviceProvider, pattern));
            }

            catch (Exception e)
            {
                
            }
        }
        
        private bool AnimationCompleted()
        {
            return _playPatternTask == null || _playPatternTask.Status == TaskStatus.Canceled ||
                   _playPatternTask.Status == TaskStatus.Created || _playPatternTask.Status == TaskStatus.RanToCompletion;
        }

        public void PlayAll(PatternOptions options)
        {
            if (!AnimationCompleted()) return;
            _playPatternTask = new Task(() =>
            {
                foreach (var pattern in _patterns)
                {
                    int total = new Random(Guid.NewGuid().GetHashCode()).Next(1, 5);
                    pattern.Project(options);
                }
            }, TaskCreationOptions.RunContinuationsAsynchronously);

            _playPatternTask.Start();
        }

        /// <summary>
        /// Plays the specified patternOptions, optional duration animation will stop after set milliseconds
        /// </summary>
        /// <param name="patternName"></param>
        /// <param name="animationSpeed"></param>
        /// <param name="duration"></param>
        public void PlayPattern(PatternOptions options)
        {
            ILaserPattern pattern = _patterns.Find(p => p.GetType().Name == options.PatternName);

            if (!AnimationCompleted()) return;
            _playPatternTask = new Task(() =>
            {
                if (pattern == null) return;

                pattern.Project(options);

            }, TaskCreationOptions.RunContinuationsAsynchronously);

            _playPatternTask.Start();
        }
    }
}