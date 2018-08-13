using SystemBase;
using UniRx;

namespace Systems.Score
{
    public class ScoreComponent : GameComponent
    {
        public IntReactiveProperty LovedOnes = new IntReactiveProperty(0);
        public IntReactiveProperty AngryOnes = new IntReactiveProperty(0);
    }
}