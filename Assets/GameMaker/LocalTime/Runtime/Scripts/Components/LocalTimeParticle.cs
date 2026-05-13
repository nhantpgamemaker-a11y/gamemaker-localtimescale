using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    [RequireComponent(typeof(ParticleSystem))]
    public class LocalTimeParticle : MonoBehaviour, ILocalTimeComponent
    {
        [SerializeField]
        [HideInInspector]
        private float _baseSimulationSpeed = 1f;
        private float _currentTimeScale = 1f;
        private ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem
        {
            get
            {
                if (_particleSystem == null) _particleSystem = GetComponent<ParticleSystem>();
                return _particleSystem;
            }
        }
        private void OnValidate()
        {
            var main = ParticleSystem.main;
            main.simulationSpeed = _baseSimulationSpeed;
        }
        
        public void OnLocalTimeChanged(float timeScale)
        {
             _currentTimeScale = timeScale;
            var main = ParticleSystem.main;
            main.simulationSpeed = _currentTimeScale;
        }
    }
}