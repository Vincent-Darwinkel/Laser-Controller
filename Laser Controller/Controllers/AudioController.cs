using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Laser_Controller.Controllers
{
    [Route("api/audio")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly AudioLogic _audioLogic;
        private readonly JsonHandler _jsonHandler;

        public AudioController(AudioLogic audioLogic, JsonHandler jsonHandler)
        {
            _audioLogic = audioLogic;
            _jsonHandler = jsonHandler;
        }

        [HttpPost("start")]
        public void StartAudio()
        {
           _audioLogic.StartAudioAlgorithm();
        }

        [HttpPost("stop")]
        public void StopAudio()
        {
            _audioLogic.StopAudioAlgorithm();
        }

        [HttpPost("calibrate/{calibrationValue}")]
        public async Task CalibrateAudioVolume(float calibrationValue)
        {
            _audioLogic._audioCalibrationValue = calibrationValue;
            var audioSettings = new AudioSettings
            {
                AudioCalibrationValue = calibrationValue
            };

            await _jsonHandler.Save(audioSettings, StoragePath.audio);
        }
    }
}