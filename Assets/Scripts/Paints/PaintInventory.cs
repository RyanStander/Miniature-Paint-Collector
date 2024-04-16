using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models.Data.Player;
using UnityEngine;

namespace Paints
{
    public class PaintInventory
    {
        private Dictionary<int, float> paintQuantities;
        private List<int> wishlistedPaints;
        public bool IsInitialized;

        #region Setup

        public PaintInventory()
        {
            paintQuantities = new Dictionary<int, float>();
            wishlistedPaints = new List<int>();
            var paintQuantitiesTask = LoadPaintQuantities();
            var wishlistedPaintsTask = LoadWishlistedPaints();
            
            IsDataLoaded(paintQuantitiesTask, wishlistedPaintsTask);
        }

        private async Task LoadPaintQuantities()
        {
            var playerData =
                await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "PaintQuantities" });
            if (playerData.TryGetValue("PaintQuantities", out var keyName))
            {
                paintQuantities = new Dictionary<int, float>(keyName.Value.GetAs<Dictionary<int, float>>());
            }
        }

        private async Task LoadWishlistedPaints()
        {
            var playerData =
                await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "WishlistedPaints" });
            if (playerData.TryGetValue("WishlistedPaints", out var keyName))
            {
                wishlistedPaints = new List<int>(keyName.Value.GetAs<List<int>>());
            }
        }
        
        private async void IsDataLoaded(Task paintQuantitiesTask, Task wishlistedPaintsTask)
        {
            await Task.WhenAll(paintQuantitiesTask, wishlistedPaintsTask);
            
            IsInitialized = true;
        }

        #endregion

        #region Inventory Methods

        public async void SetPaintQuantity(int paintID, float quantity)
        {
            //if the quantity is 0, remove the paint from the dictionary
            if (quantity <= 0)
                paintQuantities.Remove(paintID);
            else
                paintQuantities[paintID] = quantity; // Update the dictionary

            await SavePaintQuantities();
        }

        public float GetPaintQuantity(int paintID)
        {
            return paintQuantities.TryGetValue(paintID, out var quantity) ? quantity : 0f;
        }

        public Dictionary<int, float> GetPaintQuantities()
        {
            return paintQuantities;
        }

        #endregion

        #region Wishlist Methods

        public bool HasPaintWishlisted(int paintID)
        {
            return wishlistedPaints.Contains(paintID);
        }

        public List<int> GetWishlistedPaints()
        {
            return wishlistedPaints;
        }

        public async void WishlistPaint(int paintID)
        {
            if (wishlistedPaints.Contains(paintID))
                wishlistedPaints.Remove(paintID);
            else
                wishlistedPaints.Add(paintID);

            await SaveWishlistedPaints();
        }

        #endregion

        #region Save Methods

        private async Task SavePaintQuantities()
        {
            var data = new Dictionary<string, object> { { "PaintQuantities", paintQuantities } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        private async Task SaveWishlistedPaints()
        {
            var data = new Dictionary<string, object> { { "WishlistedPaints", wishlistedPaints } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        #endregion

        #region Deletion Methods

        public void ClearData()
        {
            DeletePaintQuantityData();
            DeleteWishlistedPaints();
        }

        //Inventory
        private async void DeletePaintQuantityData()
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync("PaintQuantities");
            paintQuantities.Clear(); // Clear local data as well
        }

        //Wishlist
        private async void DeleteWishlistedPaints()
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync("WishlistedPaints");
            wishlistedPaints.Clear(); // Clear local data as well
        }

        #endregion
    }
}

/*
    Documentation: https://docs.unity.com/ugs/en-us/manual/cloud-save/manual
*/
