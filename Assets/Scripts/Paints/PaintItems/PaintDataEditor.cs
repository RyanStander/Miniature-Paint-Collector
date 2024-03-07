using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EditMode")]

namespace Paints.PaintItems
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class PaintDataEditor
    {
        static PaintDataEditor()
        {
            EditorApplication.delayCall += EnsureUniqueIDs;
        }

        internal static void EnsureUniqueIDs()
        {
            PaintData[] paints = Resources.LoadAll<PaintData>(""); // Load all PaintData assets

            if (paints.Length == 0)
                return;

            // Sort paints by ID
            List<PaintData> sortedPaints = new List<PaintData>(paints);
            sortedPaints.Sort((x, y) => x.PaintItem.ID.CompareTo(y.PaintItem.ID));

            // Dictionary to keep track of duplicate IDs
            Dictionary<int, List<PaintData>> duplicateIDMap = new Dictionary<int, List<PaintData>>();

            // Find duplicate IDs
            foreach (PaintData paint in sortedPaints)
            {
                if (!duplicateIDMap.ContainsKey(paint.PaintItem.ID))
                {
                    duplicateIDMap.Add(paint.PaintItem.ID, new List<PaintData>());
                }

                duplicateIDMap[paint.PaintItem.ID].Add(paint);
            }

            // Assign unique IDs to duplicates
            foreach (var kvp in duplicateIDMap)
            {
                List<PaintData> paintsWithDuplicateID = kvp.Value;
                if (paintsWithDuplicateID.Count > 1)
                {
                    int uniqueID = kvp.Key;
                    foreach (PaintData paint in paintsWithDuplicateID)
                    {
                        // Skip if the ID is already unique
                        if (!IsUniqueID(paints, uniqueID))
                        {
                            uniqueID = GetNextAvailableID(paints);
                        }

                        paint.PaintItem.ID = uniqueID;
                        uniqueID++;
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // Check if the ID is unique in the array of paints
        private static bool IsUniqueID(PaintData[] paints, int id)
        {
            foreach (PaintData paint in paints)
            {
                if (paint.PaintItem.ID == id)
                {
                    return false;
                }
            }

            return true;
        }

        // Get the next available ID
        private static int GetNextAvailableID(PaintData[] paints)
        {
            int maxID = 0;
            foreach (PaintData paint in paints)
            {
                if (paint.PaintItem.ID > maxID)
                {
                    maxID = paint.PaintItem.ID;
                }
            }

            return maxID + 1;
        }
    }
#endif
}
