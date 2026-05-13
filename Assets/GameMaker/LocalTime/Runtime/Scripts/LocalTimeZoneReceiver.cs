using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    public enum LocalTimeZoneType
    {
        Min,
        Max,
        Override
    }
    [RequireComponent(typeof(LocalTimeAgent))]
    [DisallowMultipleComponent]
    public class LocalTimeZoneReceiver: MonoBehaviour, ILocalTimeZoneReceiver
    {
        [SerializeField] private LocalTimeAgent _agent;
        [UnityEngine.SerializeField] private LocalTimeZoneType _zoneType = LocalTimeZoneType.Min;

        private float _currentTimeScale = 1f;


        private List<LocalTimeZone> _localTimeZones = new List<LocalTimeZone>();

        public void OnAddZone(LocalTimeZone zone)
        {
            _localTimeZones.Add(zone);
            _currentTimeScale = CalTimeScale();
            _agent.SetTimeScaleCurve(_currentTimeScale);
        }

        public void OnRemoveZone(LocalTimeZone zone)
        {
            _localTimeZones.Remove(zone);
            _currentTimeScale = CalTimeScale();
            _agent.SetTimeScaleCurve(_currentTimeScale);
        }

        private float CalTimeScale()
        {
            if (_localTimeZones.Count == 0) return 1f;
            switch (_zoneType)
            {
                case LocalTimeZoneType.Min:
                    return _localTimeZones.Min(z => z.TimeScale);
                case LocalTimeZoneType.Max:
                    return _localTimeZones.Max(z => z.TimeScale);
                case LocalTimeZoneType.Override:
                    return _localTimeZones[_localTimeZones.Count - 1].TimeScale;
                default:
                    return 1f;
            }
        }
    }
}