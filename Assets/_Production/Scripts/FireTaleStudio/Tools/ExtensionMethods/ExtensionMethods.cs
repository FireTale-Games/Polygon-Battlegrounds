using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FTS.Tools.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static T Null<T>(this T self) where T : Object => self ? self : null;
        
        public static void ShowCanvasGroup(this CanvasGroup self, float fadeDuration, Action onStart = null, Action onComplete = null)
        {
            if (self == null)
                return;
            
            self.DOFade(1, fadeDuration).OnStart(() => onStart?.Invoke()).OnComplete(() =>
            {
                self.interactable = true;
                self.blocksRaycasts = true;
                onComplete?.Invoke();
            }).Play();
        }

        public static void HideCanvasGroup(this CanvasGroup self, float fadeDuration, Action onStart = null, Action onComplete = null) =>
            self.DOFade(0, fadeDuration).OnStart(() => {
                    self.interactable = false;
                    self.blocksRaycasts = false;
                    onStart?.Invoke();
                }).OnComplete(() => onComplete?.Invoke()).Play();
        
        public static string AddSpaceBetweenCapitalLetters(this string self) =>
            self.First() + new string(self.Skip(1).SelectMany(c => char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray());
    }
}