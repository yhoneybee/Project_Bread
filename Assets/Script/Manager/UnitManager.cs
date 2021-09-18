using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; } = null;

    public Dictionary<string, List<Unit>> Pool = new Dictionary<string, List<Unit>>();
    public List<Unit> Units = new List<Unit>();

    private void Awake()
    {
        if (Instance != null)
            Instance = this;
    }
    private void Start()
    {
        Units.AddRange(Resources.LoadAll<Unit>("/Unit"));
    }

    public Unit GetUnit(string name, Vector2 pos)
    {
        Unit unit;

        if (Pool.ContainsKey(name) && Pool[name].Count > 0)
        {
            unit = Pool[name][0];
            Pool[name].Remove(unit);
        }
        else
        {
            unit = Instantiate(Units.Find((o) => { return o.Info.Name == name; }), pos, Quaternion.identity);
        }

        unit.transform.position = pos;
        unit.gameObject.SetActive(true);

        return unit;
    }
    public void ReturnUnit(Unit unit)
    {
        unit.Stat.HP = unit.Stat.MaxHP;
        Pool[unit.Info.Name].Add(unit);
        unit.gameObject.SetActive(false);
    }
}