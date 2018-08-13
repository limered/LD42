using Systems.GameState;
using UniRx;
using UnityEngine;

namespace Systems.Interface
{
    public class StartButtonComponent : MonoBehaviour
    {
        public void StartGameClick()
        {
            MessageBroker.Default.Publish(new GameMessageStart());
        }
    }
}
