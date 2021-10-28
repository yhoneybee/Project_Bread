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
    public List<Tuple<Type, string>> SaveDataInfos;
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

    public bool IsFile(string file_name) => File.Exists($"{Application.persistentDataPath}/{file_name}.txt");

    public static void Save<T>(IEnumerable<T> save_target, string file)
    {
        var datas = save_target.Cast<T>();

        string json = JsonUtility.ToJson(new Serialization<T>(datas.ToList()));

        Instance.Text_file_name = file;
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(Instance.Text_file_name, code);
        File.WriteAllText($"{Application.persistentDataPath}/{file}_Log.txt", json);

        var find = Instance.SaveDataInfos.Find((o) => o.Item2 == file);
        if (find != null) Instance.SaveDataInfos.Add(new Tuple<Type, string>(typeof(T), file));

        print($"SAVE TO : {Instance.Text_file_name}");
    }

    /// <summary>
    /// 그냥 List의 정보 그대로 불러올때 사용하는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="load_target"></param>
    public static void Load<T>(ref List<T> load_target, string file)
    {
        Instance.Text_file_name = file;
        if (!File.Exists(Instance.Text_file_name)) { Instance.DefaultSetting("Obsolete"); return; }

        string code = File.ReadAllText(Instance.Text_file_name);
        byte[] bytes = Convert.FromBase64String(code);
        string json = System.Text.Encoding.UTF8.GetString(bytes);

        load_target = JsonUtility.FromJson<Serialization<T>>(json).target;

        print($"LOAD FROM : {Instance.Text_file_name}");
    }

    /// <summary>
    /// List안에 멤버에 접근할 경우 불러오는 List를 return하여 따로 대입해줘야 하는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
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

    private void OnApplicationQuit()
    {

    }
}
