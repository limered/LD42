using SystemBase;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Interface
{
    public class IntefaceComponent : GameComponent
    {
        public GameObject StartScreen;
        public GameObject GuiScreen;
        public GameObject EndScreen;

        public Text ScoreLovedText;
        public Text ScoreAngryText;

        public Text EndLovedText;
        public Text EndAngryText;
    }
}