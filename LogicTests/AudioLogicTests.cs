using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Tests
{
    [TestClass()]
    public class AudioLogicTests
    {
        [TestMethod()]
        public void AudioLogicTest()
        {
            AudioLogic audioLogic = new AudioLogic(null);
            Task.Run(() => audioLogic.StartAudioAlgorithm());

            audioLogic._AlgorithmCanceled = true;
        }
    }
}