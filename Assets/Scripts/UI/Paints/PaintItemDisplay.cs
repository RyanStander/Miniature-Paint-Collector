using Paints.PaintItems;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Paints
{
    public class PaintItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image paintImage;
        [SerializeField] private TextMeshProUGUI paintName;
        [SerializeField] private Image paintColor;
        
        public void SetPaintData(PaintData paintData)
        {
            paintImage.sprite = paintData.PaintItem.PaintSprite;
            paintName.text = paintData.PaintItem.Name;
            paintColor.color = paintData.PaintItem.PaintColor;
        }
    }
}
