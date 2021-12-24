using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; } = null;

    public Dictionary<string, List<Unit>> Pool = new Dictionary<string, List<Unit>>();
    public List<Unit> Units = new List<Unit>();
    public List<SkillInfo> skillInfos = new List<SkillInfo>();

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        string[] paths = { "1. COMMON", "2. RARE", "3. EPIC", "4. LEGEND" };

        for (int i = 0; i < paths.Length; i++)
            Units.AddRange(Resources.LoadAll<Unit>("Unit/" + paths[i]));

        if (SaveManager.Instance.IsFile("SkillDatas"))
        {
            SaveManager.SaveToByteFromLog("SkillDatas");
            SaveManager.Load(ref skillInfos, "SkillDatas");
        }
        else
        {
            List<SkillInfo> skillInfossssss = new List<SkillInfo>();

            foreach (var item in Units)
                skillInfossssss.Add(new SkillInfo { name = item.Info.Name, tick = 0.1f, duraction = 1, duractionObj = 1 });

            SaveManager.Save(skillInfossssss, "SkillDatas");
        }

        for (int i = 0; i < GameManager.Instance.theme_count; i++)
            Units.AddRange(Resources.LoadAll<Unit>($"Unit/Enemy/Theme {i + 1}"));
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

                if (unit != null)
                {
                    foreach (var item in unit.Items)
                        if (item != null)
                        {
                            item.Owner = unit;
                            item.Ingame();
                        }
                }
            }
        }

        if (unit == null)
        {
            var find = Units.Find((o) => { return o.Info.Name == name; });
            unit = Instantiate(find, pos, Quaternion.identity);

            for (int i = 0; i < find.Items.Count; i++)
            {
                var item = Instantiate(find.Items[i], unit.transform, false);
                item.Owner = unit;
                item.Ingame();
                unit.Items[i] = item;
            }
        }

        unit.transform.position = pos;
        unit.Init();
        unit.gameObject.SetActive(true);
        if (unit.SR) unit.SR.color = Color.white;
        else unit.GetComponent<SpriteRenderer>().color = Color.white;

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
        IngameManager.Instance.IngameUnits.Remove(unit);
        IngameManager.Instance.IngameUnits.Remove(unit);
    }
}