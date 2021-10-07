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
        Units.AddRange(Resources.LoadAll<Unit>("Unit"));
        Units.AddRange(Resources.LoadAll<Unit>("Unit/Enemy"));
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
    }

    public Unit GetUnit(string name, Vector2 pos)
    {
        Unit unit = null;

        if (Pool.ContainsKey(name))
        {
            Pool[name].RemoveAll(o => o == null);

            if (Pool[name].Count > 0)
            {
                unit = Pool[name][0];
                Pool[name].Remove(unit);
            }
        }

        if (unit == null)
            unit = Instantiate(Units.Find((o) => { return o.Info.Name == name; }), pos, Quaternion.identity);

        unit.transform.position = pos;
        unit.Init();
        unit.gameObject.SetActive(true);

        return unit;
    }
    public void ReturnUnit(Unit unit, Transform parent)
    {
        if (!Pool.ContainsKey(unit.Info.Name))
            Pool.Add(unit.Info.Name, new List<Unit>());
        Pool[unit.Info.Name].Add(unit);
        if (parent != null)
            unit.transform.SetParent(parent);
        unit.gameObject.SetActive(false);
    }
}