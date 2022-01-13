using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "RandomSpawn", menuName = "Datas/Options/RandomSpawn")]
public class RandomSpawn : ASpawnOption
{
    [SerializeField] private List<GameObject> origins;
    [SerializeField] private List<int> damages;

    public override IEnumerator EInvoke(Unit unit)
    {
        int idx = Random.Range(0, origins.Count - 1);
        var origin = origins[idx];

        var obj = Instantiate(origin);
        var spawned = obj.AddComponent<DamagedObj>();
        spawned.context = context;
        spawned.transform.position = context.owner.transform.position;
        spawned.damage = damages[idx];

        yield return null;
    }
}