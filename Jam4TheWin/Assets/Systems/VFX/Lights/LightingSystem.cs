using System.Collections;
using SystemBase;
using Systems.Player;
using Systems.Player.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.VFX.Lights
{
    [GameSystem(typeof(PlayerSystem))]
    public class LightingSystem : GameSystem<CatComponent, PartyLightComponent, FoodLightComponent, LooLightComponent>
    {
        private readonly Color _activeColor = Color.green;
        private readonly Color _inactiveColor = Color.red;

        private ReactiveProperty<CatComponent> _cat = new ReactiveProperty<CatComponent>();

        public override void Register(CatComponent component)
        {
            _cat.Value = component;
        }

        public override void Register(PartyLightComponent component)
        {
        }

        public override void Register(FoodLightComponent component)
        {
            if (_cat.HasValue)
            {
                _cat.Subscribe(RegisterFoodLightToCatState(component)).AddTo(component);
            }
            else
            {
                _cat.Skip(1).Subscribe(RegisterFoodLightToCatState(component)).AddTo(component);
            }
        }

        public override void Register(LooLightComponent component)
        {
            if (_cat.HasValue)
            {
                _cat.Subscribe(RegisterLooLightToCatState(component)).AddTo(component);
            }
            else
            {
                _cat.Skip(1).Subscribe(RegisterLooLightToCatState(component)).AddTo(component);
            }
        }

        private IEnumerator BlinkLight(float duration, float min, float max, Light light)
        {
            for (float i = 0; i < duration; i += Time.deltaTime)
            {
                var step = (max - min * i / duration);
                var intensity = min + step;
                light.intensity = intensity;
                yield return null;
            }
        }

        private System.Action<CatComponent> RegisterFoodLightToCatState(FoodLightComponent component)
        {
            return cat => cat.CatStateContext.CurrentState.Subscribe(state =>
            {
                if (state.GetType() == typeof(Hungry))
                {
                    if (component.Dispo != null)
                    {
                        component.Dispo.Dispose();
                        component.Dispo = null;
                    }
                    component.GetComponent<Light>().color = _activeColor;
                    component.GetComponent<Light>().intensity = 15;
                }
                else if (state.GetType() == typeof(Eating))
                {
                    component.Dispo = component.UpdateAsObservable()
                        .Subscribe(unit =>
                        {
                            var l = component.GetComponent<Light>();
                            var intensity = Mathf.Cos(Time.realtimeSinceStartup * 5) * 10 + 5;
                            intensity = Mathf.Max(0, intensity);
                            intensity = Mathf.Min(15, intensity);
                            l.intensity = intensity;
                        });
                }
                else
                {
                    if (component.Dispo != null)
                    {
                        component.Dispo.Dispose();
                        component.Dispo = null;
                    }
                    component.GetComponent<Light>().color = _inactiveColor;
                    component.GetComponent<Light>().intensity = 15;
                }
            });
        }
        private System.Action<CatComponent> RegisterLooLightToCatState(LooLightComponent component)
        {
            return cat => cat.CatStateContext.CurrentState.Subscribe(state =>
            {
                if (state.GetType() == typeof(Full))
                {
                    if (component.Dispo != null)
                    {
                        component.Dispo.Dispose();
                        component.Dispo = null;
                    }
                    component.GetComponent<Light>().color = _activeColor;
                    component.GetComponent<Light>().intensity = 15;
                }
                else if (state.GetType() == typeof(Pooping))
                {
                    component.Dispo = component.UpdateAsObservable()
                        .Subscribe(unit =>
                        {
                            var l = component.GetComponent<Light>();
                            var intensity = Mathf.Cos(Time.realtimeSinceStartup * 3) * 10 + 5;
                            intensity = Mathf.Max(0, intensity);
                            intensity = Mathf.Min(15, intensity);
                            l.intensity = intensity;
                        });
                }
                else
                {
                    if (component.Dispo != null)
                    {
                        component.Dispo.Dispose();
                        component.Dispo = null;
                    }
                    component.GetComponent<Light>().color = _inactiveColor;
                    component.GetComponent<Light>().intensity = 15;
                }
            });
        }
    }
}