using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic
{
    public class PatternLogic
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<ILaserPattern> _patterns = new List<ILaserPattern>();

        public PatternLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var patterns = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var pattern in patterns)
                _patterns.Add((ILaserPattern)ActivatorUtilities.CreateInstance(_serviceProvider, pattern));
        }

        public void PlayAll()
        {
            foreach (var pattern in _patterns)
            {
                int total = new Random(Guid.NewGuid().GetHashCode()).Next(1, 5);
             
                var task = new Task(() => pattern.Project(total), TaskCreationOptions.LongRunning);
                task.Start();
                task.Wait();
            }
        }

        public void PlayPattern(string patternName)
        {
            ILaserPattern pattern = _patterns.Find(p => p.GetType().Name == patternName);
            if (pattern == null) return;

            int total = new Random(Guid.NewGuid().GetHashCode()).Next(1, 5);
            var task = new Task(() => pattern.Project(total), TaskCreationOptions.LongRunning);
            task.Start();
            task.Wait();
        }
    }
}