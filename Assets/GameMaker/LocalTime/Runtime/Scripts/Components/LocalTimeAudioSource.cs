using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class LocalTimeAudioSource : MonoBehaviour, ILocalTimeComponent
    {
        [HideInInspector][SerializeField] private float _basePitch = 1f;
        private float _currentTimeScale = 1f;
        private AudioSource _audioSource;
        public AudioSource AudioSource
        {
            get
            {
                if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
                return _audioSource;
            }
        }
        private void OnValidate()
        {
            _basePitch = AudioSource.pitch;
        }
        
        public void OnLocalTimeChanged(float timeScale)
        {
            _currentTimeScale = timeScale;
            AudioSource.pitch = _basePitch * _currentTimeScale;
        }
    }
}