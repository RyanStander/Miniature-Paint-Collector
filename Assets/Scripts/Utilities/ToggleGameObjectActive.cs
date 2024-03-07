using UnityEngine;

namespace Utilities
{
    public class ToggleGameObjectActive : MonoBehaviour
    {
        [SerializeField] private GameObject gameObjectToToggle;
        
        public void ToggleGameObject()
        {
            gameObjectToToggle.SetActive(!gameObjectToToggle.activeSelf);
        }
    }
}
