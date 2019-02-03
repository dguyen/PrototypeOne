using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity: MonoBehaviour, IEntity {
    
    // public string shortDescription;
    // public string longDescription;
    public Sprite entitySprite;

    private List<Capability> avaliableCapabilities = new List<Capability>();

    public void AddCapability(Capability toAdd) {
        if (!avaliableCapabilities.Contains(toAdd)) {
            avaliableCapabilities.Add(toAdd);
        }
    }

    public void RemoveCapability(Capability toRemove) {
        if (avaliableCapabilities.Contains(toRemove)) {
            avaliableCapabilities.Remove(toRemove);
        }
    }

    public bool HasCapability(Capability capability) {
        return avaliableCapabilities.Contains(capability);
    }

    public List<Capability> GetCapabilities() {
        return avaliableCapabilities;
    }

    public Sprite GetSprite() {
        return entitySprite;
    }
}
