using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Ads
{
    public class AppMenuAdDisplays : MonoBehaviour
    {
        [SerializeField] private float bannerAdDisplayInterval = 30f;
        [SerializeField] private float interstitialAdDisplayInterval = 180f;

        private float bannerAdTimeStamp;
        private float interstitialAdTimeStamp;
        private bool interstitialLoading = false;
        
        private void Start()
        {
            //load a banner ad and display it
            AdmobAdsScript.LoadBannerAd();
            bannerAdTimeStamp = Time.time + bannerAdDisplayInterval;
            interstitialAdTimeStamp = Time.time + interstitialAdDisplayInterval;
        }

        private void Update()
        {
            if (Time.time > bannerAdTimeStamp)
            {
                bannerAdTimeStamp = Time.time + bannerAdDisplayInterval;
                //load a banner ad and display it
                AdmobAdsScript.LoadBannerAd();
            }
            
            if (Time.time > interstitialAdTimeStamp && !interstitialLoading)
            {
                //dont want to start the timer again until the ad is closed
                interstitialAdTimeStamp = Time.time + interstitialAdDisplayInterval+10000f;
                interstitialLoading = true;
                LoadInterstitialAd();
            }
        }
        
        private async void LoadInterstitialAd()
        {
            AdmobAdsScript.LoadInterstitialAd();
            
            var adShown = false;
            var adClosed = false;
            
            while (!adClosed)
            {
                if (!adShown && AdmobAdsScript.ShowInterstitialAd())
                {
                    adShown = true;
                    //After showing ad wait for event where ad is closed
                    AdmobAdsScript.interstitialAd.OnAdFullScreenContentClosed += () =>
                    {
                        adClosed = true;
                        interstitialLoading = false;
                        interstitialAdTimeStamp = Time.time + interstitialAdDisplayInterval;
                        //unload ad screen
                        AdmobAdsScript.DestroyInterstitialAd();
                    };
                }
                else
                {
                    await Task.Yield();
                }
            }
        }
    }
}
