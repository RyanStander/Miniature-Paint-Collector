using System;
using UnityEngine;

namespace Animations
{
    public class AnimatorCanvasGroupEvents : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private void OnValidate()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        // This function is called from the animation event
        public void OnAnimationEndHideObject()
        {
            if (!canvasGroup) 
                return;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
