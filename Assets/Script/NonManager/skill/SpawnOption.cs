using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(fileName = "SpawnOption", menuName = "Datas/Options/SpawnOption")]
public class SpawnOption : ASpawnOption
{
    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(spawnDelay);

        for (int i = 0; origin && i < spawnCount; i++)
        {
            var obj = Instantiate(origin);
            var spawned = obj.GetComponent<SpawnedObj>();
            if (parentToSkillOwner)
            {
                spawned.transform.SetParent(context.transform);
            }
            spawned.context = context;

            if (isNear)
            {
                obj.transform.position = new Vector3(offset.x + UnityEngine.Random.Range(-range, range), offset.y + UnityEngine.Random.Range(-range, range));
            }
            else
            {
                obj.transform.position = new Vector3(context.owner.transform.position.x + offset.x + (cell.x + spacing.x) * i, context.owner.transform.position.y + offset.y + (cell.y + spacing.y) * i, -1);
            }
            if (!isOnce) yield return wait;
        }

        yield return null;
    }
}