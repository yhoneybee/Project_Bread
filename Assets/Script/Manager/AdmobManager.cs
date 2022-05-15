using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance { get; private set; } = null;

    //private InterstitialAd interstitialAd;
    //private RewardedAd rewardedAd;
    public bool isTestMode = true;

    public ProductType rewardKind;
    public int rewardValue;

    bool rewareded;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //MobileAds.Initialize(x => { });
        //RequestInterstitial(); //전면광고 초기화
        //RequestRewardedAd();   //보상형 광고 초기화
    }

    private void Update()
    {
        if (rewareded)
        {
            switch (rewardKind)
            {
                case ProductType.COIN:
                    GameManager.Instance.Coin += rewardValue;
                    break;
                case ProductType.JEM:
                    GameManager.Instance.Jem += rewardValue;
                    break;
                case ProductType.STEMINA:
                    GameManager.Instance.Stemina += rewardValue;
                    break;
            }
            UIManager.Instance.rewardInfoLinker.Show(rewardKind, rewardValue);
            rewareded = false;
        }
    }

    private void RequestInterstitial()
    {
        //string adUnitId = isTestMode ? "ca-app-pub-3940256099942544/1033173712" : "ca-app-pub-5708876822263347/8389822452";
        //interstitialAd = new InterstitialAd(adUnitId);
        //interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //interstitialAd.OnAdClosed += HandleOnAdClosed;
        //AdRequest request = new AdRequest.Builder().Build();
        //interstitialAd.LoadAd(request);
    }

    //public void HandleOnAdClosed(object sender, EventArgs e)
    //{
    //    interstitialAd.Destroy();
    //    RequestInterstitial();
    //}

    //public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //{
    //    Debug.Log("failed to load ad");
    //    //todo : 광고 로드에 실패했을 때 다른광고로드
    //}

    //public void ShowInterstitial()
    //{
    //    if (interstitialAd.IsLoaded())
    //    {
    //        interstitialAd.Show();
    //    }
    //    else
    //    {
    //        RequestInterstitial();
    //    }
    //}

    //private void RequestRewardedAd()
    //{
    //    string adUnitId = isTestMode ? "ca-app-pub-3940256099942544/5224354917" : "ca-app-pub-5708876822263347/9492236277";
    //    rewardedAd = new RewardedAd(adUnitId);
    //    rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToShow;
    //    rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    //    rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    //    AdRequest request = new AdRequest.Builder().Build();
    //    rewardedAd.LoadAd(request);
    //}

    //public void HandleRewardedAdFailedToShow(object sender, AdFailedToLoadEventArgs args)
    //{
    //    //todo : 에러났을때 다른광고 띄우는 것 구현
    //}

    //public void HandleRewardedAdClosed(object sender, EventArgs args)
    //{
    //    RequestRewardedAd();
    //}

    //public void HandleUserEarnedReward(object sender, Reward args)
    //{
    //    string type = args.Type;
    //    double amount = args.Amount;
    //    print($"type({type}) / amount({amount})");
    //    rewareded = true;
    //    //todo : 보상설정
    //}

    //public void ShowRewardedAd()
    //{
    //    if (rewardedAd.IsLoaded())
    //    {
    //        rewardedAd.Show();
    //    }
    //    else
    //    {
    //        RequestRewardedAd();
    //    }
    //}
}
