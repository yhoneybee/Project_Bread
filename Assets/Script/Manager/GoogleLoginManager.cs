using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleLoginManager : MonoBehaviour
{
    public static GoogleLoginManager Instance { get; private set; } = null;

    private void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayLogin();
    }

    public void PlayLogin()
    {
        //���� ����ڰ� �����Ǿ����� Ȯ���մϴ�
        if (!Social.localUser.authenticated)
        {
            //���� Ȱ�� Social API ������ ���� ���� ����ڸ� �����ϰ� ���� ������ �����͸� �����ɴϴ�
            //ù��° ���� : �������� / �ι�° ���� : ���н� ���� �α�
            Social.localUser.Authenticate((bool isOk, string error) =>
            {
            });
        }
    }

    public void PlayLogout()
    {
        //Social.Active : ���� Ȱ��ȭ�� �Ҽ� �÷���(���� ��Ȳ������ PlayGamesPlatform)�� ��ȯ
        PlayGamesPlatform platform = Social.Active as PlayGamesPlatform;
        platform.SignOut();
    }
}
