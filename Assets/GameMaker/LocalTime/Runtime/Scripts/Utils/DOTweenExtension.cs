using System;
using UnityEngine;
#if LOCAL_TIME_DOTWEEN_SUPPORT
using DG.Tweening;

namespace GameMaker.LocalTime.Runtime
{
    public static class DOTweenExtension
    {
        public static T BindTimeScale<T>(this T tween, Func<float> timeScaleGetter) where T : Tween
        {
            DOTween.To(() => 0, x => {}, 0, 1)
            .SetLoops(-1)
            .SetUpdate(true)
            .OnUpdate(() => 
            {
                if (tween == null || !tween.IsActive()) return;
                tween.timeScale = timeScaleGetter();
            })
            .SetTarget(tween);
        }
    }
}
#endif