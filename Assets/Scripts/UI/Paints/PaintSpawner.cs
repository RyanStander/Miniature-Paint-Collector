using System;
using System.Collections.Generic;
using System.Linq;
using Paints.PaintItems;
using UnityEngine;

namespace UI.Paints
{
    public class PaintSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject paintItemPrefab;
        [SerializeField] private PaintBrandContainer[] paintBrandContainers;
        private List<int> paintBrandsWithPaints = new List<int>();

        private void OnValidate()
        {
            //find all paint brand containers
            paintBrandContainers = GetComponentsInChildren<PaintBrandContainer>();
        }

        public void SpawnPlayerCollection(Dictionary<int, float> paintQuantities)
        {
            // Clear all paint brand containers
            foreach (var brandContainer in paintBrandContainers)
            {
                brandContainer.ClearContainer();
            }

            // Load all PaintData assets from Resources folder
            var paintDatas = Resources.LoadAll<PaintData>("Paints");

            // Iterate through each PaintData and spawn paint items
            foreach (var paintData in paintDatas)
            {
                //Skip paint if it's not in the player's collection
                if (!paintQuantities.ContainsKey(paintData.PaintItem.ID))
                    continue;

                for (var index = 0; index < paintBrandContainers.Length; index++)
                {
                    var brandContainer = paintBrandContainers[index];
                    if (brandContainer.PaintBrand == paintData.PaintItem.Brand)
                    {
                        var paintItem = Instantiate(paintItemPrefab, brandContainer.ContentTransform);
                        var paintItemComponent = paintItem.GetComponent<PaintItemDisplay>();
                        if (paintItemComponent != null)
                        {
                            paintItemComponent.SetPaintData(paintData);
                        }
                        else
                        {
                            Debug.LogError("PaintItem component not found on prefab.");
                        }

                        break;
                    }

                    if (!paintBrandsWithPaints.Contains(index))
                    {
                        paintBrandsWithPaints.Add(index);
                    }
                        
                }
            }

            //Hide empty paint brand containers
            for ( var paintBrandContainersIndex = 0; paintBrandContainersIndex < paintBrandContainers.Length; paintBrandContainersIndex++)
            {
                if (!paintBrandsWithPaints.Contains(paintBrandContainersIndex))
                {
                    paintBrandContainers[paintBrandContainersIndex].gameObject.SetActive(false);
                }
            }
        }

        public void ShowAllPaints()
        {
            // Clear all paint brand containers
            foreach (var brandContainer in paintBrandContainers)
            {
                brandContainer.ClearContainer();
            }

            // Load all PaintData assets from Resources folder
            var paintDatas = Resources.LoadAll<PaintData>("Paints");

            // Iterate through each PaintData and spawn paint items
            foreach (var paintData in paintDatas)
            {
                foreach (var paintItemComponent in from brandContainer in paintBrandContainers
                         where brandContainer.PaintBrand == paintData.PaintItem.Brand
                         select Instantiate(paintItemPrefab, brandContainer.ContentTransform)
                         into paintItem
                         select paintItem.GetComponent<PaintItemDisplay>())
                {
                    if (paintItemComponent != null)
                    {
                        paintItemComponent.SetPaintData(paintData);
                    }
                    else
                    {
                        Debug.LogError("PaintItem component not found on prefab.");
                    }

                    break;
                }
            }
        }
    }
}
