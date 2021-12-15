using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

[Serializable]
public class Serialization<T>
{
    public Serialization(List<T> targets) => target = targets;
    public List<T> target;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance = null;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    public string GetFilePath(string file) => $"{Application.persistentDataPath}/{file}.txt";

    public bool IsFile(string file_name) => File.Exists($"{Application.persistentDataPath}/{file_name}.txt");

    public static void Save<T>(T save_target, string file)
    {
        string path = Instance.GetFilePath(file);

        string json = JsonUtility.ToJson(save_target);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(path, code);
        File.WriteAllText($"{Application.persistentDataPath}/{file}_Log.txt", json);
    }

    public static void Save<T>(IEnumerable<T> save_target, string file)
    {
        string path = Instance.GetFilePath(file);

        var datas = save_target.Cast<T>();

        string json = JsonUtility.ToJson(new Serialization<T>(datas.ToList()));

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(path, code);
        File.WriteAllText($"{Application.persistentDataPath}/{file}_Log.txt", json);

        //print($"SAVE TO : {path}");
    }

    public static void SaveUnits(IEnumerable<Unit> save_units, string file) => GameManager.Instance.onAutoSave += () =>
    {
        Save(save_units.Select((o) =>
        {
            if (o != null) return o.Info.Name;
            return "___";
        }).ToList(), file);
    };

    /// <summary>
    /// �׳� List�� ���� �״�� �ҷ��ö� ����ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="load_target"></param>
    public static void Load<T>(ref List<T> load_target, string file)
    {
        string path = Instance.GetFilePath(file);

        if (!File.Exists(path)) { return; }

        string code = File.ReadAllText(path);
        byte[] bytes = Convert.FromBase64String(code);
        string json = System.Text.Encoding.UTF8.GetString(bytes);

        load_target = JsonUtility.FromJson<Serialization<T>>(json).target;

        //print($"LOAD FROM : {path}");
    }

    public static void LoadUnits(ref List<Unit> load_units, string file)
    {
        var loads = Load<string>(file);
        for (int i = 0; i < loads.Count(); i++)
        {
            var find = UnitManager.Instance.Units.Find((o) => loads.ElementAt(i) == o.Info.Name);
            if (find != null) load_units[i] = find;
        }
    }

    /// <summary>
    /// List�ȿ� ����� ������ ��� �ҷ����� List�� return�Ͽ� ���� ��������� �ϴ� �Լ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
    public static IEnumerable<T> Load<T>(string file = "")
    {
        string path = Instance.GetFilePath(file);

        if (!File.Exists(path)) { return null; }

        string code = File.ReadAllText(path);
        byte[] bytes = Convert.FromBase64String(code);
        string json = System.Text.Encoding.UTF8.GetString(bytes);

        //print($"LOAD FROM : {path}");

        return JsonUtility.FromJson<Serialization<T>>(json).target;
    }

    public static T Load<T>(string file = "", bool _ = true)
    {
        string path = Instance.GetFilePath(file);

        if (!File.Exists(path)) { return default(T); }

        string code = File.ReadAllText(path);
        byte[] bytes = Convert.FromBase64String(code);
        string json = System.Text.Encoding.UTF8.GetString(bytes);

        return JsonUtility.FromJson<T>(json);
    }
}
