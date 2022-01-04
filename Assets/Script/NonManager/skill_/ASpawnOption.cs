using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ASpawnOption : AOption
{
    public abstract bool IsGuide { get; set; }
    public abstract float Speed { get; set; }
    public abstract float Duraction { get; set; }
}
