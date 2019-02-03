using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeGun : Entity {
    // Start is called before the first frame update
    void Start() {
        base.AddCapability(Capability.PICKABLE);
    }
}
