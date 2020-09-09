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

        [HttpGet("patterns")]
        public List<string> GetPatterns()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ILaserPattern).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
        }

        [HttpPost("all")]
        public void PlayAll([FromBody] PatternOptions options)
        {
            new Task(() => _patternLogic.PlayAll(options), TaskCreationOptions.RunContinuationsAsynchronously).Start();
        }

        [HttpPost("play")]
        public void PlayPattern([FromBody] PatternOptions options)
{
            new Task(() => _patternLogic.PlayPattern(options), TaskCreationOptions.RunContinuationsAsynchronously).Start();
        }
    }
}