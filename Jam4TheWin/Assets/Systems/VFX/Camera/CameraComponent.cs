using SystemBase;
using UnityEngine;

namespace Systems.VFX
{
    public class CameraComponent : GameComponent
    {
        public Color NextColor;
        public Color CurrentColor;

        public Color LoveColor;
        public float LoveRandomness;
    }
}