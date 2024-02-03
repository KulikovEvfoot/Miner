using System.Collections;
using UnityEngine;

namespace Common
{
    public class CoroutineProvider : MonoBehaviour, ICoroutineRunner
    {
        private static CoroutineProvider m_Instance;

        public static CoroutineProvider Instance
        {
            get
            {
                if (m_Instance != null)
                {
                    return m_Instance;
                }

                m_Instance = new GameObject("CoroutineProvider").AddComponent<CoroutineProvider>();
                DontDestroyOnLoad(m_Instance.gameObject);

                return m_Instance;
            }
        }
        
        public Coroutine DoCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public void Stop(Coroutine coroutine)
        {
            Instance.StopCoroutine(coroutine);
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}