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
        public void StartAudio()
        {
           _audioLogic.StartAudioAlgorithm();
        }

        [HttpPost("stop")]
        public void StopAudio()
        {
            _audioLogic.StopAudioAlgorithm();
        }
    }
}
