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

            // Iterate over paints, ensuring unique and incremental IDs
            int nextID = 0;
            foreach (PaintData paint in sortedPaints)
            {
                // Ensure negative or duplicate IDs are updated
                if (paint.PaintItem.ID < 0 || usedIDs.Contains(paint.PaintItem.ID))
                {
                    paint.PaintItem.ID = nextID;
                }

                // Find the next available ID
                while (usedIDs.Contains(nextID))
                {
                    nextID++;
                } 
 
                // Assign new unique ID
                EditorUtility.SetDirty(paint);
                paint.PaintItem.ID = nextID;
                usedIDs.Add(nextID);
                nextID++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
#endif
}
