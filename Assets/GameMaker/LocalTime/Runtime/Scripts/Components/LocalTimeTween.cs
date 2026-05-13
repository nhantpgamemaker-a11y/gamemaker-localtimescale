using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    public class LocalTimeTween : MonoBehaviour, ILocalTimeComponent
    {
        private float _currentTimeScale = 1f;
        public float CurrentTimeScale => _currentTimeScale;
        public void OnLocalTimeChanged(float timeScale)
        {
           _currentTimeScale = timeScale;
        }
    }
}
