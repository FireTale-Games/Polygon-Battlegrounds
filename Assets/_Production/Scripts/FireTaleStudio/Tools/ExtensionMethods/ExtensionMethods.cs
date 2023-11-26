using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FTS.Tools.ScriptableEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace FTS.Tools.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static T Null<T>(this T self) where T : Object => self ? self : null;
        
        public static void ShowCanvasGroup(this CanvasGroup self, float fadeDuration, Action onStart = null, Action onComplete = null) =>
            self.DOFade(1, fadeDuration).OnStart(() => onStart?.Invoke()).OnComplete(() => {
                self.interactable = true;
                self.blocksRaycasts = true;
                onComplete?.Invoke();
            }).Play();

        public static void HideCanvasGroup(this CanvasGroup self, float fadeDuration, Action onStart = null, Action onComplete = null) =>
            self.DOFade(0, fadeDuration).OnStart(() => {
                    self.interactable = false;
                    self.blocksRaycasts = false;
                    onStart?.Invoke();
                }).OnComplete(() => onComplete?.Invoke()).Play();

        public static EventInvoker<T> LoadEventInvoker<T>(string name) => 
            Resources.Load<EventInvoker<T>>($"{typeof(T).Name}/{name}");
        
        public static GameObject FirstRaycastHit()
        {
            List<RaycastResult> tempList = new();
            EventSystem.current.Null()?.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, tempList);
            return tempList.FirstOrDefault().gameObject;
        }
    }
}