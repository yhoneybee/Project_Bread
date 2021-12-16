using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObj : MonoBehaviour
{
    public virtual void Excute(Collider2D col2D)
    {

    }
    public virtual void UnExcute(Collider2D col2D)
    {

    }

    private void OnTriggerEnter2D(Collider2D col2D)
    {
        Excute(col2D);
    }

    private void OnTriggerExit2D(Collider2D col2D)
    {
        UnExcute(col2D);
    }
}
