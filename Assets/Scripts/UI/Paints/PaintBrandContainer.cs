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

        public void ClearContainer()
        {
            foreach (Transform child in ContentTransform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
