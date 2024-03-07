using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("EditMode")]
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

            // List to keep track of used IDs
            HashSet<int> usedIDs = new HashSet<int>();

            // Find the lowest available ID
            int nextID = 1; // Start from 1, as IDs should not be less than 1
            foreach (PaintData paint in sortedPaints)
            {
                // Ensure negative or duplicate IDs are updated
                if (paint.PaintItem.ID < 1 || usedIDs.Contains(paint.PaintItem.ID))
                {
                    while (usedIDs.Contains(nextID))
                    {
                        nextID++;
                    }
                    paint.PaintItem.ID = nextID;
                }

                usedIDs.Add(paint.PaintItem.ID);
                nextID++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } 
    }
#endif
}
