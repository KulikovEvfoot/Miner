using Services.Navigation.Runtime.Scripts.Transfer;
using UnityEngine;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public class ResourceExtractorBody : MonoBehaviour, IMovableBody
    {
        public Transform Transform => gameObject.transform;
    }
}