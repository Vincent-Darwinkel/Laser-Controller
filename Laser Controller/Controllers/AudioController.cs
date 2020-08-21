using System.Collections.Generic;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Mvc;

namespace Laser_Controller.Controllers
{
    [Route("api/audio")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly AudioLogic _audioLogic;

        public AudioController(AudioLogic audioLogic)
        {
            _audioLogic = audioLogic;
        }

        [HttpPost("start")]
        public async Task StartAudio([FromBody] string deviceName)
        {
            await Task.Run(() => _audioLogic.StartAudioAlgorithm(deviceName));
        }

        [HttpPost("stop")]
        public void StopAudio()
        {
            _audioLogic._algorithmCanceled = true;
        }

        [HttpGet("devices")]
        public List<string> GetAudioDevices()
        {
            return _audioLogic.GetAudioDevices();
        }
    }
}
