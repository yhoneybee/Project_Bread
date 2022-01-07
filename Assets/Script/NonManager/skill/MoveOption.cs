using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "MoveOption", menuName = "Datas/Options/MoveOption")]
public class MoveOption : AMoveOption
{
    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(0.001f);

        float time = 0;

        float ms = context.owner.Stat.MS;

        while (time <= duraction)
        {
            if (isTrigger)
                context.owner.transform.Translate(Vector3.right * speed * Time.deltaTime);
            else
                context.owner.Stat.MS = speed;

            time += 0.001f;
            yield return wait;
        }

        context.owner.Stat.MS = ms;

        yield return null;
    }
}