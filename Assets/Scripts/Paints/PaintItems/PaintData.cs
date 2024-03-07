using UnityEngine;

namespace Paints.PaintItems
{
    [CreateAssetMenu(fileName = "New Paint", menuName = "Scriptable Objects/Paint Data/Paint")]
    public class PaintData : ScriptableObject
    {
        public PaintItem PaintItem = new();

        private void OnValidate()
        {
            //if name is empty, set it to the name of the asset with spaces where cammel case is, also make sure each word starts with a capital letter
            if (string.IsNullOrEmpty(PaintItem.Name))
            {
                PaintItem.Name = name;
                PaintItem.Name = System.Text.RegularExpressions.Regex.Replace(PaintItem.Name, "(\\B[A-Z])", " $1");
                PaintItem.Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PaintItem.Name);
            }
        }
    }
}
