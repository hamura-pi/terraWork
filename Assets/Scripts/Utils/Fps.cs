using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class Fps : MonoBehaviour
    {
        public float UpdateInterval = 0.5f;

        private float _accum; // FPS accumulated over the interval
        private int _frames; // Frames drawn over the interval
        private float _timeleft; // Left time for current interval
        public UnityEngine.UI.Text Label;

        public void Start()
        {

            _timeleft = UpdateInterval;
        }

        public void Update()
        {
            _timeleft -= Time.deltaTime;
            _accum += Time.timeScale/Time.deltaTime;
            ++_frames;

            // Interval ended - update GUI text and start new interval
            if (_timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                Label.text = SystemInfo.graphicsDeviceType + " " + (_accum/_frames).ToString("f2") + "\n" +
                             Screen.width + "x" + Screen.height;
                _timeleft = UpdateInterval;
                _accum = 0.0f;
                _frames = 0;
            }
        }
    }
}