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
        public bool IsInitialized;

        public PaintInventory()
        {
            paintQuantities = new Dictionary<int, float>();
            LoadPaintQuantities();
        }

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

        private async Task SavePaintQuantities()
        {
            var data = new Dictionary<string, object> { { "PaintQuantities", paintQuantities } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        private async void LoadPaintQuantities()
        {
            var playerData =
                await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "PaintQuantities" });
            IsInitialized = true;
            if (playerData.TryGetValue("PaintQuantities", out var keyName))
            {
                paintQuantities = new Dictionary<int, float>(keyName.Value.GetAs<Dictionary<int, float>>());
            }
        }

        public async void DeletePaintQuantityData()
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync("PaintQuantities");
            paintQuantities.Clear(); // Clear local data as well
        }
    }
}

/*
    Data saving example:

    var data = new Dictionary<string, object>{ { "MySaveKey", "HelloWorld" } };
    await CloudSaveService.Instance.Data.ForceSaveAsync(data);

    Documentation: https://docs.unity.com/ugs/en-us/manual/cloud-save/manual
*/
