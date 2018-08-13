using System;
using SystemBase;
using UniRx;

namespace Systems.Score
{
    [GameSystem]
    public class ScoreSystem : GameSystem<ScoreComponent>
    {
        private int _angryPersons;
        private int _lovedPersons;
        private int _maxScore = 42;

        public override void Register(ScoreComponent component)
        {
            MessageBroker.Default.Receive<MessagePersonLoved>()
                .Subscribe(IncreaseLoved(component))
                .AddTo(component);

            MessageBroker.Default.Receive<MessagePersonLeftAngry>()
                .Subscribe(IncreseAngry(component))
                .AddTo(component);
        }

        private void CheckForEnd()
        {
            if (_lovedPersons + _angryPersons == _maxScore)
            {
                MessageBroker.Default.Publish(new MessageMaxScoreReached());
            }
        }

        private Action<MessagePersonLoved> IncreaseLoved(ScoreComponent component)
        {
            return loved =>
            {
                _lovedPersons++;
                component.LovedOnes.Value = _lovedPersons;
                CheckForEnd();
            };
        }

        private Action<MessagePersonLeftAngry> IncreseAngry(ScoreComponent component)
        {
            return angry =>
            {
                _angryPersons++;
                component.AngryOnes.Value = _angryPersons;
                CheckForEnd();
            };
        }
    }
}