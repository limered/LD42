using SystemBase;
using Systems.GameState;
using Systems.Score;
using UniRx;
using Utils;

namespace Systems.Interface
{
    [GameSystem]
    public class InterfaceSystem : GameSystem<IntefaceComponent, ScoreComponent>
    {
        private readonly StringReactiveProperty _sAngryOnes = new StringReactiveProperty("0");
        private readonly StringReactiveProperty _sLovedOnes = new StringReactiveProperty("0");
        public override void Register(IntefaceComponent component)
        {
            IoC.Game.GameStateMachine.CurrentState
                .Where(state => state.GetType() == typeof(StartScreen))
                .Select(s=>component)
                .Subscribe(ShowStartScreen)
                .AddTo(component);

            IoC.Game.GameStateMachine.CurrentState
                .Where(state => state.GetType() == typeof(Running))
                .Select(s => component)
                .Subscribe(HideStartScreen)
                .AddTo(component);

            IoC.Game.GameStateMachine.CurrentState
                .Where(state => state.GetType() == typeof(GameOver))
                .Select(s => component)
                .Subscribe(ShowEndScreen)
                .AddTo(component);

            IoC.Game.GameStateMachine.CurrentState
                .Where(state => state.GetType() == typeof(StartScreen))
                .Select(s => component)
                .Subscribe(HideEndScreen)
                .AddTo(component);

            _sLovedOnes.Subscribe(s=>
            {
                component.ScoreLovedText.text = s;
                component.EndLovedText.text = s;

            }).AddTo(component);

            _sAngryOnes.Subscribe(s=>
            {
                component.ScoreAngryText.text = s;
                component.EndAngryText.text = s;

            }).AddTo(component);
        }

        public override void Register(ScoreComponent component)
        {
            component.LovedOnes.Subscribe(i => _sLovedOnes.Value = i.ToString()).AddTo(component);
            component.AngryOnes.Subscribe(i => _sAngryOnes.Value = i.ToString()).AddTo(component);
        }

        private void HideEndScreen(IntefaceComponent inteface)
        {
            inteface.EndScreen.SetActive(false);
        }

        private void HideStartScreen(IntefaceComponent inteface)
        {
            inteface.StartScreen.SetActive(false);
            inteface.GuiScreen.SetActive(true);
        }

        private void ShowEndScreen(IntefaceComponent inteface)
        {
            inteface.EndScreen.SetActive(true);
        }

        private void ShowStartScreen(IntefaceComponent inteface)
        {
            inteface.StartScreen.SetActive(true);
            inteface.GuiScreen.SetActive(false);
        }
    }
}
