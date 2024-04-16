using System;
using Events;
using Paints.PaintItems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Paints
{
    public class AddPaintDisplay : MonoBehaviour
    {
        [SerializeField] private Image paintDisplay;
        [SerializeField] private Image paintDisplayMaskImage;
        [SerializeField] private Mask paintDisplayMask;
        [SerializeField] private TextMeshProUGUI paintNameText;
        [SerializeField] private TextMeshProUGUI paintBrandText;
        [SerializeField] private TextMeshProUGUI paintQuantityText;

        [Header("Auto Assigned")] [SerializeField]
        private CanvasGroup canvasGroup;

        private PaintData selectedPaint;
        private float paintQuantity;

        private void OnValidate()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventIdentifiers.OpenPaintContextMenu, OnOpenPaintContextMenu);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventIdentifiers.OpenPaintContextMenu, OnOpenPaintContextMenu);
        }

        private void SetSelectedPaint(PaintData paintData, float quantity)
        {
            selectedPaint = paintData;
            paintQuantity = quantity;
            
            if (paintData.PaintItem.PaintSpriteMask != null)
                paintDisplayMaskImage.sprite = paintData.PaintItem.PaintSpriteMask;
            else
            {
                paintDisplayMaskImage.enabled = false;
                paintDisplayMask.enabled = false;
            }

            paintDisplay.sprite = paintData.PaintItem.PaintSprite;
            paintDisplay.color = quantity < 1 ? Color.gray : Color.white;

            paintNameText.text = paintData.PaintItem.Name;
            paintBrandText.text = paintData.PaintItem.Brand.ToString();
            paintQuantityText.text = quantity.ToString();
            OpenDisplay();
        }

        public void AddPaint()
        {
            paintQuantity++;

            if (paintQuantity > 0)
                paintDisplay.color = Color.white;
            
            paintQuantityText.text = paintQuantity.ToString();

            EventManager.currentManager.AddEvent(new SetPlayerPaintQuantity(selectedPaint.PaintItem.ID, paintQuantity));
        }

        public void RemovePaint()
        {
            if (paintQuantity < 1)
                return;

            paintQuantity--;

            if (paintQuantity < 1)
                paintDisplay.color = Color.gray;
            
            paintQuantityText.text = paintQuantity.ToString();

            EventManager.currentManager.AddEvent(new SetPlayerPaintQuantity(selectedPaint.PaintItem.ID, paintQuantity));
        }
        
        public void WishlistPaint()
        {
            EventManager.currentManager.AddEvent(new WishlistPaint(selectedPaint.PaintItem.ID));
        }

        public void CloseDisplay()
        {
            selectedPaint = null;
            paintQuantity = 0;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void OpenDisplay()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        
        private void OnOpenPaintContextMenu(EventData eventData)
        {
            if (!eventData.IsEventOfType<OpenPaintContextMenu>(out var openPaintContextMenu))
                return;

            SetSelectedPaint(openPaintContextMenu.PaintData, openPaintContextMenu.Quantity);
        }
    }
}
