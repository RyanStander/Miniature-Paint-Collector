using UI.Paints;

namespace Utilities
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SortByHue
    {
        private List<PaintItemDisplay> paintItemDisplays;
        private List<PaintItemDisplay> sortedColors = new();
        private List<float> hues = new();

        public List<PaintItemDisplay> SortColorsByHue(List<PaintItemDisplay> givenPaintItemDisplays)
        {
            paintItemDisplays = givenPaintItemDisplays;
            hues.Clear();
            sortedColors.Clear();

            foreach (var paintItemDisplay in paintItemDisplays)
            {
                var hsv = Vector3.zero;
                Color.RGBToHSV(paintItemDisplay.PaintData.PaintItem.PaintColor, out hsv.x, out hsv.y, out hsv.z);
                hues.Add(hsv.x * 360);

                sortedColors.Add(paintItemDisplay);
            }

            Reorder();

            return paintItemDisplays;
        }

        private void Reorder()
        {
            for (var i = 0; i < hues.Count; i++)
            {
                if (i > 0)
                {
                    if (hues[i] < hues[i - 1])
                    {
                        sortedColors.Add(sortedColors[i - 1]);
                        hues.Add(hues[i - 1]);
                        sortedColors.RemoveAt(i - 1);
                        hues.RemoveAt(i - 1);

                        Reorder();
                    }
                    else if (i == hues.Count - 1)
                    {
                        paintItemDisplays = sortedColors;
                    }
                }
            }
        }
    }
}
