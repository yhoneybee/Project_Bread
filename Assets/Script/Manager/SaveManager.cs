using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

[Serializable]
public class Serialization<T>
{
    public Serialization(List<T> target) => this.target = target;
    public List<T> target;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance = null;
    string file_path;
    public string Text_file_name
    {
        get { return file_path; }
        set
        {
            file_path = $"{Application.persistentDataPath}/{value}.txt";
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    void DefaultSetting(string file)
    {
        //switch (file)
        //{
        //    case "BreadData":
        //        foreach (var item in GameManager.Instance.All_Units)
        //            item.Data.Count = 0;

        //        Save(GameManager.Instance.All_Units.Select(o => o.Data), "BreadData");
        //        var load = Load<BreadData>("BreadData");
        //        for (int i = 0; i < load.Count(); i++)
        //            GameManager.Instance.All_Units[i].Data = load.ElementAt(i);
        //        break;
        //    case string s when s.Contains("Deck"):

        //        break;
        //}
    }

    /// <summary>
    /// 좀더 보완된 Save함수
    /// </summary>
    /// <typeparam name="T">저장할 자료형</typeparam>
    /// <param name="save_target">저장되는 타겟(T의 필드(변수)에 접근하려면 linq로 넘기면 된다)</param>
    public static void Save<T>(IEnumerable<T> save_target, string file = "")
    {
        var datas = save_target.Cast<T>();

        string json = JsonUtility.ToJson(new Serialization<T>(datas.ToList()));

        Instance.Text_file_name = file;
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(Instance.Text_file_name, code);
        print($"SAVE TO : {Instance.Text_file_name}");
    }

    /// <summary>
    /// 좀더 보완된 Load함수
    /// </summary>
    /// <typeparam name="T">불러올 자료형</typeparam>
    /// <param name="load_target">불러와서 갱신할 타겟</param>
    public void Load<T>(ref List<T> load_target)
    {
        Text_file_name = typeof(T).Name;
        if (!File.Exists(Text_file_name)) { DefaultSetting("Obsolete"); return; }

        string code = File.ReadAllText(Text_file_name);
        byte[] bytes = Convert.FromBase64String(code);
        string json = System.Text.Encoding.UTF8.GetString(bytes);

        load_target = JsonUtility.FromJson<Serialization<T>>(json).target;

        print($"LOAD FROM : {Text_file_name}");
    }

    /// <summary>
    /// list를 return해서 직접 대입해야하는 Load함수
    /// </summary>
    /// <typeparam name="T">저장된 타입</typeparam>
    /// <returns>저장된 정보의 IEnumerable</returns>
    public static IEnumerable<T> Load<T>(string file = "")
    {
        Instance.Text_file_name = file;
        if (!File.Exists(Instance.Text_file_name)) { Instance.DefaultSetting(file); return null; }

        string code = File.ReadAllText(Instance.Text_file_name);
        byte[] bytes = Convert.FromBase64String(code);
        string json = System.Text.Encoding.UTF8.GetString(bytes);

        print($"LOAD FROM : {Instance.Text_file_name}");

        return JsonUtility.FromJson<Serialization<T>>(json).target;
    }
}
