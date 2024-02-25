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
        [SerializeField] private List<PaintBrandContainer> paintBrandContainers;

        private void OnValidate()
        {
            //find all paint brand containers
            paintBrandContainers = new List<PaintBrandContainer>(GetComponentsInChildren<PaintBrandContainer>());
        }

        private void Start()
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
