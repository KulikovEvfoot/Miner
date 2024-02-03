using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameStarter : IInitializable
    {
        private readonly List<IInitPhase> m_InitPhases;
        private readonly List<IInitPhaseCompleteListener> m_InitPhaseCompleteListeners;

        [Inject]
        public GameStarter(List<IInitPhase> initPhases, List<IInitPhaseCompleteListener> initPhaseCompleteListeners)
        {
            m_InitPhases = initPhases;
            m_InitPhaseCompleteListeners = initPhaseCompleteListeners;
        }

        public async void Initialize()
        {
            var tasks = m_InitPhases.Select(initPhase => initPhase.Init()).ToArray();
            await Task.WhenAll(tasks);
            Debug.Log($"Init complete. {tasks.Count()} objects inited. Start game!");

            foreach (var listener in m_InitPhaseCompleteListeners)
            {
                listener.NotifyOnAllInitComplete();
            }
        }
    }
}