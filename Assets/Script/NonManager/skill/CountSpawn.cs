using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "CountSpawn", menuName = "Datas/Options/CountSpawn")]
public class CountSpawn : ASpawnOption
{
    public override IEnumerator EInvoke(Unit unit)
    {
        var wait = new WaitForSeconds(spawnDelay);
        int count = IngameManager.Instance.IngameUnits.Count * 2;
        if (count == 2) count = 3;
        for (int i = 0; i < count; i++)
        {
            if (IngameManager.Instance.IngameUnits.Count == 0) break;
            var obj = Instantiate(origin);
            var spawned = obj.AddComponent<SpawnedObj>();
            spawned.context = context;

            if (isNear)
            {
                obj.transform.position = new Vector3(offset.x + Random.Range(-range, range), offset.y + Random.Range(-range, range));
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
