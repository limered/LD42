﻿using SystemBase;
using Systems.Movement;
using UniRx;
using UnityEngine;

namespace Systems.Player
{
    [RequireComponent(typeof(TargetMutator))]
    public class CatComponent : GameComponent
    {
        public static float MaxHunger = 100;
        public static float MaxLoveTimer = 0.15f;
        public static float MaxPoopingTime = 5f;

        public FloatReactiveProperty Hunger = new FloatReactiveProperty(MaxHunger);
        public FloatReactiveProperty PoopingTimer = new FloatReactiveProperty(MaxPoopingTime);
        public FloatReactiveProperty LoveTimer = new FloatReactiveProperty(MaxLoveTimer);

        public float AngryTime;

        public GameObject LoveCollider;
        public GameObject StinkCollider;
        public GameObject InnerSpaceCollider;

        public CatStateContext CatStateContext;
        public bool IsAngry { get; set; }
        public float InLoveStarted { get; set; }
        public static float MaxInLoveTime = 15;
    }
}