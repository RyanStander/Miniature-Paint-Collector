using System;
using UnityEngine;

namespace Paints.PaintItems
{
    [Serializable]
    public class PaintItem
    {
        #region Fields

        public int ID;
        public string Name;
        public PaintBrand Brand;
        public Sprite PaintSprite;
        public Color PaintColor = Color.white;

        #endregion
    }
}
