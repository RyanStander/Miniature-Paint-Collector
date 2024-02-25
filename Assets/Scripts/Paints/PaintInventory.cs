using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models.Data.Player;
using UnityEngine;

namespace Paints
{
    public class PaintInventory
    {
        // Method to add paint quantity to inventory
        public async void AddPaintQuantity(int paintID, float quantity)
        {
            // Get the current quantity for the paint ID
            float currentQuantity = await GetPaintQuantity(paintID);

            // Add the new quantity
            float newQuantity = currentQuantity + quantity;

            // Save the new quantity directly as a float
            var data = new Dictionary<string, object> { { "Paint_" + paintID, newQuantity } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        // Method to get paint quantity from inventory
        public async Task<float> GetPaintQuantity(int paintID)
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Paint_" + paintID });

            // Retrieve and parse the quantity as a float
            if (playerData.TryGetValue("Paint_" + paintID, out var keyName))
            {
                float quantity;
                if (float.TryParse(keyName.Value.GetAs<string>(), out quantity))
                {
                    return quantity;
                }
                else
                {
                    Debug.LogWarning("Invalid quantity data for Paint_" + paintID + ". Returning 0.");
                }
            }

            // Return 0 if not found or invalid
            return 0f;
        }

        public async void DeletePaintQuantityData(int paintID)
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync("Paint_" + paintID);
        }
    }
}

/*
    Data saving example:

    var data = new Dictionary<string, object>{ { "MySaveKey", "HelloWorld" } };
    await CloudSaveService.Instance.Data.ForceSaveAsync(data);

    Documentation: https://docs.unity.com/ugs/en-us/manual/cloud-save/manual
*/
