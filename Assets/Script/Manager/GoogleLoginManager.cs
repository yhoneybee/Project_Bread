using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Firebase.Auth;

public class GoogleLoginManager : MonoBehaviour
{
    private FirebaseAuth auth;
    public string FireBaseId = string.Empty;

    private void Awake()
    {

    }

    //void Start()
    //{
    //    m_saved_state = GoogleSavedFileState.GOOGLE_SAVED_STATE_NONE;


    //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    //    // enables saving game progress.
    //    .EnableSavedGames()
    //    .RequestIdToken()
    //    .Build();

    //    PlayGamesPlatform.InitializeInstance(config);
    //    PlayGamesPlatform.DebugLogEnabled = true;
    //    PlayGamesPlatform.Activate();

    //    auth = FirebaseAuth.DefaultInstance;

    //}



    //public void Login()
    //{
    //    LogManager.Debug("GameCenter Login");
    //    if (!Social.localUser.authenticated) // �α��� �Ǿ� ���� �ʴٸ�
    //    {
    //        Social.localUser.Authenticate(success => // �α��� �õ�
    //        {
    //            if (success) // �����ϸ�
    //            {
    //                LogManager.Debug("google game service Success");
    //                //SystemMessageManager.Instance.AddMessage("google game service Success");
    //                StartCoroutine(TryFirebaseLogin()); // Firebase Login �õ�
    //            }
    //            else // �����ϸ�
    //            {
    //                LogManager.Debug("google game service Fail");
    //                SystemMessageManager.Instance.AddMessage("google game service Fail");
    //            }
    //        });
    //    }
    //}


    //public void TryGoogleLogout()
    //{
    //    if (Social.localUser.authenticated) // �α��� �Ǿ� �ִٸ�
    //    {
    //        PlayGamesPlatform.Instance.SignOut(); // Google �α׾ƿ�
    //        auth.SignOut(); // Firebase �α׾ƿ�
    //    }
    //}


    //IEnumerator TryFirebaseLogin()
    //{
    //    while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
    //        yield return null;
    //    string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();


    //    Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
    //    auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
    //        if (task.IsCanceled)
    //        {
    //            SystemMessageManager.Instance.AddMessage("SignInWithCredentialAsync was canceled!!");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            SystemMessageManager.Instance.AddMessage("SignInWithCredentialAsync encountered an error: " + task.Exception);

    //            return;
    //        }

    //        Firebase.Auth.FirebaseUser newUser = task.Result;
    //        FireBaseId = newUser.UserId;

    //        //Debug.Log("Success!");
    //        //SystemMessageManager.Instance.AddMessage("firebase Success!!");
    //    });
    //}
}
