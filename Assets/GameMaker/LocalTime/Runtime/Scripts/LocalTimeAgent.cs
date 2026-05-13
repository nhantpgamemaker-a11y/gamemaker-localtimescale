using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    [DisallowMultipleComponent]
    public class LocalTimeAgent : MonoBehaviour, ILocalTimeReceiver
    {
        [UnityEngine.SerializeField] private bool _multipleGlobalTimeScale = true;
        [UnityEngine.SerializeField] private AnimationCurve _timeScaleCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        [UnityEngine.SerializeField] private float _timeScaleCurveTime = 0f;
        private float _currentTimeScale = 1f;
        private List<ILocalTimeComponent> _components;
        private void Awake()
        {
            _components = GetComponentsInChildren<ILocalTimeComponent>().ToList();
        }
        private Coroutine _curveCoroutine;
        public float CurrentTimeScale => _currentTimeScale;
        public void SetTimeScale(float timeScale)
        {
            if (Mathf.Approximately(_currentTimeScale, timeScale)) return;
            if(_curveCoroutine != null)
            {
                StopCoroutine(_curveCoroutine);
                _curveCoroutine = null;
            }
            _currentTimeScale = timeScale;
            NotifyCurveTimeScale(_currentTimeScale);
        }
        public void SetTimeScaleCurve(float timeScale)
        {
            if (_curveCoroutine != null)
            {
                StopCoroutine(_curveCoroutine);
                _curveCoroutine = null;
            }
            _curveCoroutine = StartCoroutine(CurveTimeScaleCoroutine(timeScale));
        }
        private IEnumerator CurveTimeScaleCoroutine(float timeScale)
        {
            float startTimeScale = _currentTimeScale;
            float elapsedTime = 0f;
            while (elapsedTime < _timeScaleCurveTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / _timeScaleCurveTime);
                float curveValue = _timeScaleCurve.Evaluate(t);
                _currentTimeScale = Mathf.Lerp(startTimeScale, timeScale, curveValue);
                NotifyCurveTimeScale(_currentTimeScale);
                yield return null;
            }
            _currentTimeScale = timeScale;
            NotifyCurveTimeScale(_currentTimeScale);
            _curveCoroutine = null;
        }

        private void NotifyCurveTimeScale(float timeScale)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                var target = timeScale;
                if (_multipleGlobalTimeScale) target *= Time.timeScale;
                _components[i].OnLocalTimeChanged(target);
            }
        }
    }
}