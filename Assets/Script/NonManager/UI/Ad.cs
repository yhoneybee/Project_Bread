using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ad : MonoBehaviour
{
    public void ShowInterstitial()
    {
        AdmobManager.Instance.ShowInterstitial();
    }

    public void ShowRewardedAd()
    {
        AdmobManager.Instance.ShowRewardedAd();
    }
}
