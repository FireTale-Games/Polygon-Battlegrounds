using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FTS.Tools.ExtentionMethods
{
    public static class ExtentionMethods
    {
        public static T Null<T>(this T self) where T : Object => self ? self : null;
        
        public static void ShowCanvasGroup(this CanvasGroup self, float fadeDuration, Action onStart = null, Action onComplete = null)
        {
            self.DOFade(1, fadeDuration)
                .OnStart(() => onStart?.Invoke())
                .OnComplete(() =>
                {
                    self.interactable = true;
                    self.blocksRaycasts = true;
                    onComplete?.Invoke();
                })
                .Play();
        }

        public static void HideCanvasGroup(this CanvasGroup self, float fadeDuration, Action onStart = null, Action onComplete = null)
        {
            self.interactable = false;
            self.blocksRaycasts = false;
            self.DOFade(0, fadeDuration)
                .OnStart(() => onStart?.Invoke())
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }
    }
}