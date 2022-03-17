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
        //현재 사용자가 인증되었는지 확인합니다
        if (!Social.localUser.authenticated)
        {
            //현재 활성 Social API 구현에 대한 로컬 사용자를 인증하고 그의 프로필 데이터를 가져옵니다
            //첫번째 인자 : 성공여부 / 두번째 인자 : 실패시 오류 로그
            Social.localUser.Authenticate((bool isOk, string error) =>
            {
            });
        }
    }

    public void PlayLogout()
    {
        //Social.Active : 현재 활성화된 소셜 플랫폼(지금 상황에서는 PlayGamesPlatform)을 반환
        PlayGamesPlatform platform = Social.Active as PlayGamesPlatform;
        platform.SignOut();
    }
}
