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

    public void ChangeScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
