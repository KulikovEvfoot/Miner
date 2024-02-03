using UnityEngine;

namespace Core
{
    public class CanvasProvider : MonoBehaviour
    {
        [SerializeField] private Canvas m_UICanvas;

        public Canvas UICanvas => m_UICanvas;
    }
}