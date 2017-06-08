using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class AudioManager : MonoBehaviour {

        public static AudioManager I { get; private set; }

        public AudioSource ThemePlayer;
        public AudioSource SoundPlayer;

        public string[] Themes;

        public void Awake ()
        {
            I = this;
            DoNotDestroyedObjects.I.Add(gameObject);
            _startVolume = ThemePlayer.volume;
        }

        private float _startVolume;
        public void PlayTheme(string themeName)
        {
            ThemePlayer.DOFade(0, 1f).OnComplete(() => {
                ThemePlayer.clip = Resources.Load<AudioClip>("Audio/Themes/" + themeName);
                ThemePlayer.Play();
                ThemePlayer.DOFade(_startVolume, 1f);
            });
            
        }

        public void StopTheme()
        {
            ThemePlayer.DOFade(0, 1f).OnComplete(() => { ThemePlayer.Stop(); });
        }

        public void PlayRandomTheme()
        {
            PlayTheme(Themes[Random.Range(0, Themes.Length)]);
        }

        public void PlaySound(string soundName)
        {
            SoundPlayer.PlayOneShot(Resources.Load<AudioClip>("Audio/Sounds/" + soundName));
        }
    }
}
