using System;
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
                SpawnPaintInBrand(paintBrandContainerDictionary[paintData.PaintItem.Brand], paintData);
            }

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
        }

        private void HideEmptyPaintBrandContainers()
        {
            foreach (var paintBrandContainer in paintBrandContainerDictionary.Where(paintBrandContainer =>
                         !paintBrandsWithPaints.Contains((int)paintBrandContainer.Key)))
            {
                paintBrandContainer.Value.gameObject.SetActive(false);
            }
        }
    }
}
