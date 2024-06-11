using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class SoloPlayerSpawnEvent : SDD.Events.Event
{

}

public class DesactivateMainMenuEvent : SDD.Events.Event
{

}

public class CreateNewMissileEvent: SDD.Events.Event {
    public Transform Transform;
}

public class DestroyMissileEvent: SDD.Events.Event {
    public GameObject Missile;
}