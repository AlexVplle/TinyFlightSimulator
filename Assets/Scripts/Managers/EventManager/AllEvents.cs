using UnityEngine;

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

public class CreateSmokeEvent : SDD.Events.Event
{
    public Vector3 Position;
}

public class SmokeFadedEvent : SDD.Events.Event
{
    
}

public class DestroyFireballEvent : SDD.Events.Event
{
    public GameObject fireball;
}

public class FlightRotationUpdateEvent : SDD.Events.Event
{
    public Quaternion rotation;
    public float yaw;
}