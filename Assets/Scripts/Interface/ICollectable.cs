using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
  public bool isCollectable { get; }

  void Collect();
}
