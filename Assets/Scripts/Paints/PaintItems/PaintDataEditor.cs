using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Paints.PaintItems
{
    [InitializeOnLoad]
    public class PaintDataEditor
    {
        static PaintDataEditor()
        {
            EditorApplication.delayCall += EnsureUniqueIDs;
        }

        private static void EnsureUniqueIDs()
        {
            var paints = Resources.LoadAll<PaintData>("Paints"); // Load all PaintData assets in the paint subfolder

            if (paints.Length == 0)
                return;

            var sortedPaints = new List<PaintData>(paints);
            sortedPaints.Sort((x, y) => x.PaintItem.ID.CompareTo(y.PaintItem.ID)); // Sort by ID

            for (var i = 0; i < sortedPaints.Count; i++)
            {
                var paint = sortedPaints[i];

                if (paint.PaintItem.ID != i)
                {
                    Undo.RecordObject(paint, "Update Paint ID");
                    paint.PaintItem.ID = i;
                    EditorUtility.SetDirty(paint);
                    Debug.Log("Updated ID for paint '" + paint.name + "' to: " + i);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
