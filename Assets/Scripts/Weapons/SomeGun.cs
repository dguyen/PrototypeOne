using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeGun : Entity {
    void Start() {
        base.AddCapability(Capability.PICKABLE);
        base.AddCapability(Capability.DROPABLE);
    }
}
