using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Logic;
using Microsoft.AspNetCore.Mvc;

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
        public void PlayAll()
        {
            _patternLogic.PlayAll();
        }

        [HttpPost("play/{patternName}")]
        public void PlayPattern(string patternName)
        {
            _patternLogic.PlayPattern(patternName);
        }
    }
}