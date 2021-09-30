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
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Unit unit = Resources.Load<Unit>("Unit/2단과일케이크");
        Units.Add(unit);
    }

    public Unit GetUnit(string name, Vector2 pos)
    {
        Unit unit = null;
        bool need_unit = false;

        if (Pool.ContainsKey(name) && Pool[name].Count > 0)
        {
            if (Pool[name][0] != null)
            {
                unit = Pool[name][0];
                Pool[name].Remove(unit);
            }
            else
            {
                need_unit = true;
            }
        }
        else
        {
            need_unit = true;
        }
        if (need_unit)
        {
            unit = Instantiate(Units.Find((o) => { return o.Info.Name == name; }), pos, Quaternion.identity);
        }

        unit.transform.position = pos;
        unit.gameObject.SetActive(true);

        return unit;
    }
    public void ReturnUnit(Unit unit, Transform parent)
    {
        unit.Stat.HP = unit.Stat.MaxHP;
        if (!Pool.ContainsKey(unit.name))
            Pool.Add(unit.Info.Name, new List<Unit>());
        Pool[unit.Info.Name].Add(unit);
        unit.transform.SetParent(parent);
        unit.gameObject.SetActive(false);
    }
}