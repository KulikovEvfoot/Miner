using UnityEngine;

namespace Core.Mine.Runtime.Point.GoldResource
{
    public class GoldResourceView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_ResourceSprite;

        public void OnResourceExtracted()
        {
            m_ResourceSprite.enabled = false;
        }
    }
}