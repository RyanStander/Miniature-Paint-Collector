using System.Collections.Generic;
using Paints;
using Paints.PaintItems;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditorInternal;

namespace EditorTools.Paints
{
    public class PaintDataCreatorWindow : EditorWindow
    {
        private List<Sprite> spriteList = new();
        private PaintBrand selectedBrand = PaintBrand.None; // Assuming PaintBrand enum is defined elsewhere
        private string savePath = "Assets/Resources/Paints/";

        private Vector2 scrollPosition;

        [MenuItem("Custom Tools/Paint Data Creator")]
        public static void ShowWindow()
        {
            GetWindow<PaintDataCreatorWindow>("Paint Data Creator");
        }

        private void OnGUI()
        {
            var currentEvent = Event.current;
            GUILayout.Label("Sprite List", EditorStyles.boldLabel);

            // Allow dropping sprites into the window
            var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag Sprites Here");

            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(currentEvent.mousePosition))
                        break;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            var path = AssetDatabase.GetAssetPath(draggedObject);
                            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                            if (sprite != null)
                            {
                                spriteList.Add(sprite);
                            }
                        }
                    }

                    Event.current.Use();
                    break;
            }

            // Display the list of sprites
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (var sprite in spriteList)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(sprite.name); // Display only the name of the sprite
                if (GUILayout.Button("Remove"))
                {
                    spriteList.Remove(sprite);
                    break;
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            // Enum selection for PaintBrand
            GUILayout.Space(10);
            GUILayout.Label("Select Paint Brand:", EditorStyles.boldLabel);
            selectedBrand = (PaintBrand)EditorGUILayout.EnumPopup(selectedBrand);

            // Save path field
            GUILayout.Space(10);
            GUILayout.Label("Save Path:", EditorStyles.boldLabel);
            savePath = EditorGUILayout.TextField(savePath);

            // Button to create PaintData scriptable objects
            if (GUILayout.Button("Create Paints"))
            {
                CreatePaints();
            }
        }

        private void CreatePaints()
        {
            foreach (var sprite in spriteList)
            {
                // Create a new PaintData instance
                var paintData = ScriptableObject.CreateInstance<PaintData>();

                if (paintData == null)
                {
                    Debug.LogError("Failed to create PaintData instance");
                    return;
                }
                
                paintData.name = sprite.name;

                // Set name with spaces between capital letters
                paintData.PaintItem.Name = AddSpacesToSentence(sprite.name);

                // Set paint brand
                paintData.PaintItem.Brand = selectedBrand;

                // Set paint sprite
                paintData.PaintItem.PaintSprite = sprite;

                // Create asset file for PaintData
                var assetPath = savePath + paintData.name + ".asset";
                AssetDatabase.CreateAsset(paintData, assetPath);
            }

            // Refresh asset database to show newly created assets
            AssetDatabase.Refresh();

            // Clear sprite list after creating PaintData
            spriteList.Clear();

            // Notify user that creation is complete
            Debug.Log("Paints created successfully!");
        }

        // Function to add spaces between capital letters in a string
        private string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            var newText = new System.Text.StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}
