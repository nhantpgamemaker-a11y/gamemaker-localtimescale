using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    [RequireComponent(typeof(Animator))]
    public class LocalTimeAnimator : MonoBehaviour, ILocalTimeComponent
    {
        [HideInInspector][SerializeField] private float _baseSpeed = 1f;
        private float _currentTimeScale = 1f;
        private Animator _animator;
        public Animator Animator
        {
            get
            {
                if (_animator == null) _animator = GetComponent<Animator>();
                return _animator;
            }
        }
        private void OnValidate()
        {
            Animator.updateMode = AnimatorUpdateMode.Normal;
            _baseSpeed = Animator.speed;
        }
        
        public void OnLocalTimeChanged(float timeScale)
        {
             _currentTimeScale = timeScale;
            Animator.speed = _currentTimeScale;
        }
    }
}
