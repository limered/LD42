using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;

namespace Systems.Room
{
    public class KeepInsideRoomComponent : GameComponent
    {
        [Range(-10, 10)]
        public float customOffsetToGround = 0f;
    }
}
