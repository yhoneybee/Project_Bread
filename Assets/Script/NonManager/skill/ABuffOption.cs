using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ABuffOption : AOption
{
    public abstract bool OnlyMe { get; set; }
    public abstract bool IsInstant { get; set; }
}
