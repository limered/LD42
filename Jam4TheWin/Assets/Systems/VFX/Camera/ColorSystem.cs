using System;
using SystemBase;
using Systems.Player;
using Systems.Player.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Plugins;
using Random = UnityEngine.Random;

namespace Systems.VFX
{
    [GameSystem(typeof(PlayerSystem))]
    public class ColorSystem : GameSystem<CameraComponent, CatComponent>
    {
        private readonly ReactiveProperty<CatComponent> _cat = new ReactiveProperty<CatComponent>();

        public override void Register(CameraComponent component)
        {
            _cat.WhereNotNull()
                .Subscribe(SubscribeToCatStateChanged(component))
                .AddTo(component);

            component.UpdateAsObservable()
                .Subscribe(unit => AnimateColor(component))
                .AddTo(component);

            Observable.Interval(TimeSpan.FromSeconds(2))
                .Subscribe(t => SetNewColor(component))
                .AddTo(component);
        }

        private void AnimateColor(CameraComponent cam)
        {
            var camera = cam.GetComponent<Camera>();
            cam.CurrentColor = Color.Lerp(cam.CurrentColor, cam.NextColor, 0.02f);
            camera.backgroundColor = cam.CurrentColor;
        }

        private Action<CatComponent> SubscribeToCatStateChanged(CameraComponent component)
        {
            return cat => cat.CatStateContext.CurrentState.Subscribe(CatStateChanged(component)).AddTo(component);
        }

        private Action<ICatState> CatStateChanged(CameraComponent cam)
        {
            return state => { SetNewColor(cam); };
        }

        private void SetNewColor(CameraComponent cam)
        {
            if (_cat.Value.CatStateContext.CurrentState.Value.GetType().Name == typeof(NeedsLove).Name
                || _cat.Value.CatStateContext.CurrentState.Value.GetType().Name == typeof(Loving).Name)
            {
                var r = Random.value * cam.LoveRandomness;
                var g = Random.value * cam.LoveRandomness;
                var b = Random.value * cam.LoveRandomness;
                cam.NextColor = cam.LoveColor + new Color(r, g, b);
            }
            else
            {
                var r = Random.value;
                var g = Random.value;
                var b = Random.value;
                cam.NextColor = new Color(r, g, b);
            }
        }

        public override void Register(CatComponent component)
        {
            _cat.Value = component;
        }
    }
}