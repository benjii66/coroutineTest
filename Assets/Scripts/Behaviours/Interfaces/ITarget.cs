using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget : IStats
{
    Vector3 TargetPosition { get; }
}
