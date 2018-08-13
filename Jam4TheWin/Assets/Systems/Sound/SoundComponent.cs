using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Sound
{
    public class SoundComponent : GameComponent
    {
        [Range(0f, 2f)]
        public float MaxPitchChange = 0.25f;
        public SoundFile[] Sounds;
    }

    [Serializable]
    public struct SoundFile 
    {
        public string Name;
        public AudioClip File;

        [Range(0f, 1f)]
        public float Volume;
    }
}