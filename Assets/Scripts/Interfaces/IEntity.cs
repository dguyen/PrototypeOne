using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity {
  string GetName();
  // string GetShortDescription();
  // string GetLongDescription();
  Sprite GetSprite();
  void AddCapability(Capability toAdd);
  void RemoveCapability(Capability toRemove);
  bool HasCapability(Capability capability);
  List<Capability> GetCapabilities();
}
