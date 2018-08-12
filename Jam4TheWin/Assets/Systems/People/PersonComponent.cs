using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using Systems.Movement;
using Systems.Room;

namespace Systems.People
{
    [RequireComponent(typeof(KeepInsideRoomComponent))]
    [RequireComponent(typeof(TargetMutator))]
    public class PersonComponent : GameComponent
    {
        
    }
}
