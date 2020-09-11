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
        private readonly SerialPortModel _serialPortModel;
        private readonly LaserSettings _settings;

        public SettingsController(JsonHandler jsonHandler, SerialPortModel serialPortModel, LaserSettings settings)
        {
            _jsonHandler = jsonHandler;
            _serialPortModel = serialPortModel;
            _settings = settings;
        }

        [HttpGet("getcomports")]
        public IEnumerable<string> GetComPorts()
        {
            return _serialPortModel.GetPortNames();
        }

        [HttpPost("savesettings")]
        public async Task<Result> SaveSettings([FromBody] LaserSettings settings)
        {
            _settings.maxRight = settings.maxRight;
            _settings.maxLeft = settings.maxLeft;
            _settings.maxHeight = settings.maxHeight;
            _settings.minHeight = settings.minHeight;
            _settings.ComPort = settings.ComPort;
            _settings.maxLaserPower = settings.maxLaserPower;

            _serialPortModel.SendCommand(new SerialCommand().SaveSettings(settings));
            return await _jsonHandler.Save(settings, StoragePath.settings);
        }

        [HttpGet("getsettings")]
        public LaserSettings GetSettings()
        {
            LaserSettings settings = _serialPortModel.SendReadAndConvert<LaserSettings>(new SerialCommand().GetSettings());
            if (settings == null) return new LaserSettings
            {
                maxLaserPower = new []{0,0,0},
                ComPort = _jsonHandler.Get<LaserSettings>(StoragePath.settings).ComPort
            };

            settings.ComPort = _jsonHandler.Get<LaserSettings>(StoragePath.settings).ComPort;
            return settings;
        }
    }
}