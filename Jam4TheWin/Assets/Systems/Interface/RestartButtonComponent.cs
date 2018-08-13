using Systems.GameState;
using UniRx;
using UnityEngine;

namespace Systems.Interface
{
    public class RestartButtonComponent : MonoBehaviour
    {
        public void RestartClicked()
        {
            MessageBroker.Default.Publish(new GameMessageRestart());
        }
    }
}
