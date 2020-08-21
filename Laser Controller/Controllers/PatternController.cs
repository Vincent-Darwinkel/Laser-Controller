using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Laser_Controller.Controllers
{
    [Route("api/pattern")]
    [ApiController]
    public class PatternController : ControllerBase
    {
        private readonly PatternLogic _patternLogic;

        public PatternController(PatternLogic patternLogic)
        {
            _patternLogic = patternLogic;
        }

        [HttpPost("run")]
        public async Task RunPattern([FromBody] PatternControllerModel patternControllerModel)
        {
            AnimationSpeed animationSpeed = Enum.Parse<AnimationSpeed>(patternControllerModel.AnimationSpeed);
            _patternLogic._executePattern = true;

            await _patternLogic.ExecutePattern(patternControllerModel.PatternName, animationSpeed);
        }

        [HttpGet("patterns")]
        public IEnumerable<string> GetPatterns()
        {
            var patterns = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes())
                .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface);

            return patterns.Select(p => p.Name);
        } 
    }
}