using UnityEngine;

namespace Common
{
    public static class TransformHelper
    {
        public static void SetParentAndNormalize(this Transform rectTransform, Transform parent)
        {
            rectTransform.SetParent(parent);
            rectTransform.Normalize();
        }
        
        public static void Normalize(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;

            if (transform is RectTransform rectTransform)
            {
                rectTransform.sizeDelta = Vector2.zero;
            }
        }
    }
}