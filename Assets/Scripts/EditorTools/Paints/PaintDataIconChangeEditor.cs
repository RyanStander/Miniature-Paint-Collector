using Paints.PaintItems;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Paints
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PaintData))]
    public class PaintDataIconChangeEditor : Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            var paintData = target as PaintData;

            if (paintData == null || paintData.PaintItem.PaintSprite == null)
                return null;

            // Get the texture from the sprite
            var icon = SpriteToTexture(paintData.PaintItem.PaintSprite);
            return icon;
        }

        // Convert Sprite to Texture2D
        private Texture2D SpriteToTexture(Sprite sprite)
        {
            var texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width,
                (int)sprite.rect.height));
            texture.Apply();
            return texture;
        }
    }
#endif
}
