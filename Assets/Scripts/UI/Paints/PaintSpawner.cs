using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paints;
using Paints.PaintItems;
using UnityEngine;

namespace UI.Paints
{
    public class PaintSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject paintItemPrefab;
        [SerializeField] private PaintBrandContainer[] paintBrandContainers;
        [SerializeField] private GameObject noPaintsMessage;

        private Dictionary<PaintBrand, PaintBrandContainer> paintBrandContainerDictionary = new();
        private List<int> paintBrandsWithPaints = new();

        private void OnValidate()
        {
            if (paintBrandContainers == null)
                paintBrandContainers = GetComponentsInChildren<PaintBrandContainer>();
        }

        private void Awake()
        {
            foreach (var tempPaintBrandContainer in paintBrandContainers)
            {
                paintBrandContainerDictionary.Add(tempPaintBrandContainer.PaintBrand, tempPaintBrandContainer);
            }
        }

        public void SpawnPlayerCollection(Dictionary<int, float> paintQuantities, IEnumerable<PaintData> paintDatas)
        {
            ResetPaintUIData();

            // Iterate through each PaintData and spawn paint items
            foreach (var paintData in paintDatas)
            {
                //Skip paint if it's not in the player's collection
                if (!paintQuantities.ContainsKey(paintData.PaintItem.ID))
                    continue;

                SpawnPaintInBrand(paintBrandContainerDictionary[paintData.PaintItem.Brand], paintData);
            }

            HideEmptyPaintBrandContainers();

            if (paintBrandsWithPaints.Count == 0)
            {
                noPaintsMessage.SetActive(true);
            }
        }

        public void SpawnAllPaints(IEnumerable<PaintData> paintDatas)
        {
            ResetPaintUIData();

            // Iterate through each PaintData and spawn paint items
            foreach (var paintData in paintDatas)
            {
                if (paintBrandContainerDictionary.ContainsKey(paintData.PaintItem.Brand))
                    SpawnPaintInBrand(paintBrandContainerDictionary[paintData.PaintItem.Brand], paintData);
            }

            ToggleContentActive(false);
            HideEmptyPaintBrandContainers();
        }

        private void SpawnPaintInBrand(PaintBrandContainer parent, PaintData paintData)
        {
            var paintItem = Instantiate(paintItemPrefab, parent.ContentTransform);

            if (paintItem.TryGetComponent(out PaintItemDisplay paintItemDisplay))
            {
                paintItemDisplay.SetPaintData(paintData);
            }
            else
            {
                Debug.LogError("PaintItemDisplay component not found on prefab.");
            }

            paintBrandsWithPaints.Add((int)parent.PaintBrand);
        }

        private void ResetPaintUIData()
        {
            // Clear all paint brand containers
            foreach (var brandContainer in paintBrandContainerDictionary)
            {
                brandContainer.Value.ClearContainer();
            }

            paintBrandsWithPaints = new List<int>();
            noPaintsMessage.SetActive(false);
        }

        private void HideEmptyPaintBrandContainers()
        {
            foreach (var paintBrandContainer in paintBrandContainerDictionary)
            {
                paintBrandContainer.Value.gameObject.SetActive(
                    paintBrandsWithPaints.Contains((int)paintBrandContainer.Key));
                paintBrandContainer.Value.ContentTransform.gameObject.SetActive(
                    paintBrandsWithPaints.Contains((int)paintBrandContainer.Key));
            }
        }

        public IEnumerator ToggleContentActive(bool active)
        {
            //wait for the next frame to allow the paint items to properly load
            yield return new WaitForSeconds(0.01f);
            
            foreach (var paintBrandContainer in paintBrandContainerDictionary.Values)
            {
                if (active && !paintBrandContainer.gameObject.activeSelf)
                {
                    paintBrandContainer.ContentTransform.gameObject.SetActive(false);
                }
                else
                {
                    paintBrandContainer.ContentTransform.gameObject.SetActive(active);
                }
            }
        }
    }
}
