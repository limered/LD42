using System;
using SystemBase;
using Systems.GameState;
using UniRx;
using Utils;

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
            IoC.Game.GameStateMachine.CurrentState
                .Where(state => state is StartScreen)
                .Select(s => component)
                .Subscribe(c =>
                {
                    _angryPersons = 0;
                    _lovedPersons = 0;
                    c.LovedOnes.Value = 0;
                    c.AngryOnes.Value = 0;
                })
                .AddTo(component);

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
                MessageBroker.Default.Publish(new GameMessageEnd());
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