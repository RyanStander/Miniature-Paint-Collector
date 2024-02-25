using System;
using Paints;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Paints
{
    public class PaintBrandContainer : MonoBehaviour
    {
        public PaintBrand PaintBrand;
        public Transform ContentTransform;
        
        [SerializeField] private ScrollRect scrollView;

        private void OnValidate()
        {
            if(scrollView == null)
                scrollView = GetComponent<ScrollRect>();
            
            if(scrollView!=null && ContentTransform== null)
                ContentTransform = scrollView.content;
        }

        public void ClearContainer()
        {
            foreach (Transform child in ContentTransform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
