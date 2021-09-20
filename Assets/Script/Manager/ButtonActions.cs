using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public static ButtonActions Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
    }

    public bool CheckReEntering(string name)
    {
        string scene = SceneManager.GetActiveScene().name;
        return name == scene;
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ChangeDeck(int index)
    {
        DeckManager.Instance.Index = index;
    }
}
