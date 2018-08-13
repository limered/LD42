using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Sound
{
    public class SoundComponent : GameComponent
    {
        public AudioSource SoundSource;
        public SoundFile[] Sounds;
    }

    [Serializable]
    public struct SoundFile 
    {
        public string Name;
        public AudioClip File;
    }
}