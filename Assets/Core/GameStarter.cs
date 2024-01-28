using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameStarter : IInitializable
    {
        private List<IInitPhase> m_InitPhases;

        [Inject]
        public GameStarter(List<IInitPhase> initPhases)
        {
            m_InitPhases = initPhases;
        }

        public async void Initialize()
        {
            var tasks = m_InitPhases.Select(initPhase => initPhase.Init()).ToArray();
            await Task.WhenAll(tasks);
            Debug.Log($"Init complete. {tasks.Count()} objects inited");
        }
    }
}