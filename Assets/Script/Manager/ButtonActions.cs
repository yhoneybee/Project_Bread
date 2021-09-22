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
        GameManager.Instance.Index = index;
    }
    public void ExceptUnit()
    {
        GameManager.Select[GameManager.SelectSlotIdx] = null;
        ChangeScene("C - 02 DeckSelect");
    }
}
