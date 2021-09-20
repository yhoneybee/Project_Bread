using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public void ChangeScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
