using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    public class MoveForward : MonoBehaviour
    {
        [SerializeField] private LocalTimeAgent _localTimeAgent;
        [SerializeField] private float _speed = 1f;
        private void Update()
        {
            transform.Translate(Vector3.forward * _speed * _localTimeAgent.CurrentTimeScale * Time.deltaTime);
        }
    }
}
