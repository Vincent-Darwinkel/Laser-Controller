using System.Collections.Generic;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Laser_Controller.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly JsonHandler _jsonHandler;
        private readonly SerialPortLogic _serialPortLogic;

        public SettingsController(JsonHandler jsonHandler, SerialPortLogic serialPortLogic)
        {
            _jsonHandler = jsonHandler;
            _serialPortLogic = serialPortLogic;
        }

        [HttpGet("getcomports")]
        public List<string> GetComPorts()
        {
            return _serialPortLogic.GetPortNames();
        }

        [HttpPost("savesettings")]
        public async Task<Result> SaveSettings([FromBody] Settings settings)
        {
            return await _jsonHandler.Save(settings, "Settings.json");
        }

        [HttpGet("getsettings")]
        public Settings GetSettings()
        {
            return _jsonHandler.Get<Settings>("Settings.json");
        }
    }
}