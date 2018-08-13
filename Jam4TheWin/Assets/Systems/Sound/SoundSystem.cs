using System;
using System.Collections.Generic;
using System.Linq;
using System.Sound.Messages;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Sound
{
    [GameSystem]
    public class SoundSystem : GameSystem<SoundComponent, AmbientComponent>
    {
        private class SoundComparer : IEqualityComparer<PlaySoundMessage>
        {
            public bool Equals(PlaySoundMessage x, PlaySoundMessage y)
            {
                return x.Tag != null && y.Tag != null && x.Tag == y.Tag;
            }

            public int GetHashCode(PlaySoundMessage obj)
            {
                throw new NotImplementedException("dont use this");
            }
        }

        public override void Register(SoundComponent comp)
        {
            MessageBroker.Default
                .Receive<PlaySoundMessage>()
                .DistinctUntilChanged(new SoundComparer())
                .Select(x => x.Name)
                .Subscribe(soundName =>
                {
                    if (comp.Sounds.Any(x => x.Name == soundName))
                        comp.SoundSource.PlayOneShot(comp.Sounds.First(x => x.Name == soundName).File);
                    else
                        Debug.LogWarning("sound not found: " + soundName);
                })
                .AddTo(comp);
        }

        public override void Register(AmbientComponent comp)
        {
            MessageBroker.Default
                .Receive<MuteAmbientMessage>()
                .Select(x => x.Mute)
                .Subscribe(mute => comp.AmbientAudioSource.mute = mute)
                .AddTo(comp);
        }
    }

    public static class SoundExtensions
    {
        public static void Play(this string soundName, string tag = null)
        {
            MessageBroker.Default.Publish(new PlaySoundMessage(soundName, tag));
        }
    }
}