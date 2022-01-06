using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "RandomSpawn", menuName = "Datas/Options/RandomSpawn")]
public class RandomSpawn : ASpawnOption
{
    public override Skill Context { get => context; set => context = value; }
    private Skill context;
    public override bool IsGuide { get => isGuide; set => isGuide = value; }
    public override float Speed { get => speed; set => speed = value; }
    public override float Duraction { get => duraction; set => duraction = value; }
    public override bool IsOnHit { get => isOnHit; set => isOnHit = value; }

    [SerializeField] private List<GameObject> origins;
    [SerializeField] private List<int> damages;

    [SerializeField, Header("피격시 발동 여부")]
    private bool isOnHit;
    [SerializeField, Header("소환되는 객체에 대한 값")]
    private bool isGuide;
    [SerializeField] private float speed;
    [SerializeField] private float duraction;

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
