using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GleyMobileAds;

public class AdManager : MonoBehaviour
{
   public AdManager Instance { set; get; }

   
    //int coins = 0;
    //public Text coinsText;
    //public Button intersttialButton;
    //public Button rewardedButton;

    /// <summary>
    /// Initialize the ads
    /// </summary>

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        // Initialize the Google Mobile Ads SDK.
        Advertisements.Instance.Initialize();
    }
    void Start()
    {
        //coinsText.text = coins.ToString();
    }

    /// <summary>
    /// Show banner, assigned from inspector
    /// </summary>
    public void ShawBanner()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

    public void HideBanner()
    {
        Advertisements.Instance.HideBanner();
    }


    /// <summary>
    /// Show Interstitial, assigned from inspector
    /// </summary>
    public void ShowInterstitial()
    {
        //if (CustomAdmob.Instance.IsInterstitialAvailable())
            Advertisements.Instance.ShowInterstitial();
        //else
        //    CustomAdmob.Instance.LoadInterstitial();
    }

    /// <summary>
    /// Show rewarded video, assigned from inspector
    /// </summary>
    public void ShowRewardedVideo()
    {
        //if (CustomAdmob.Instance.IsRewardVideoAvailable())
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        //else
        //    CustomAdmob.Instance.LoadRewardedVideo();
    }


    /// <summary>
    /// This is for testing purpose
    /// </summary>
    void Update()
    {
       
    }

    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            Debug.Log("You Get Reward");
        }

        
    }
}
