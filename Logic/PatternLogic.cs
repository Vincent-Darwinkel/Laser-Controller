using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic
{
    public class PatternLogic
    {
        private readonly IServiceProvider _serviceProvider;
        public List<ILaserPattern> _patternCollection { get; private set; }
        public bool _executePattern { get; set; }

        public PatternLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _patternCollection = new List<ILaserPattern>();

            var patterns = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var pattern in patterns)
                _patternCollection.Add((ILaserPattern)ActivatorUtilities.CreateInstance(_serviceProvider, pattern));
        }

        public async Task ExecutePattern(string patternName, AnimationSpeed animationSpeed)
        {
            var pattern = _patternCollection.Find(p => p.GetType().Name == patternName);

            while (_executePattern)
                pattern.Project(animationSpeed);
        }
    }
}