using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx.Triggers;
using UniRx;
using UnityEngine;


namespace Systems.Billboard
{
    [GameSystem]
    public class BillboardSystem : GameSystem<BillboardComponent>
    {
        public override void Register(BillboardComponent comp)
        {
            comp.UpdateAsObservable()
                .Subscribe(_ =>
                    {
                        if (comp.only2DRotation)
                        {
                            Vector3 v = Camera.main.transform.position - comp.transform.position;
                            v.x = v.z = 0.0f;
                            comp.transform.LookAt(Camera.main.transform.position - v);
                        }
                        else
                        {
                            comp.transform.up = Camera.main.transform.up;
                        }
                    })
                .AddTo(comp);
        }
    }
}