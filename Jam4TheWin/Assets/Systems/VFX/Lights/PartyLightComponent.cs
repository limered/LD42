using SystemBase;
using UnityEngine;

namespace Systems.VFX.Lights
{
    public class PartyLightComponent : GameComponent
    {
        public PartyLightMode Mode = PartyLightMode.Party;
        public Color LoveColor;
        public Color BaseColor;
    }
}