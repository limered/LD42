using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using System;

namespace Systems.Room
{
    public class KeepInsideRoomComponent : GameComponent
    {
        [Range(-10, 10)]
        public float customOffsetToGround = 0f;

        public float obstaclePaddig;

        public Func<Vector3, bool> CanMoveInDirection {get; set;}
    }
}
