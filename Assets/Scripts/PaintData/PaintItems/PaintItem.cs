using System;
using UnityEngine;

namespace PaintData.PaintItems
{
    [Serializable]
    public class PaintItem
    {
        #region Fields

        [SerializeField] private string name;
        [SerializeField] private PaintBrand brand;
        [SerializeField] private Sprite paintSprite;
        [SerializeField] private Color paintColor;

        #endregion
    }
}
