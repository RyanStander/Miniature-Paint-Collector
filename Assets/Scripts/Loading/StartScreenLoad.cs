using System;
using System.Collections;
using System.Threading.Tasks;
using Ads;
using GoogleMobileAds.Api;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Loading
{
    public class StartScreenLoad : LoadingScene
    {
        [SerializeField] private int sceneToLoad = 1;
        
        private bool adsLoaded;

        private void Start()
        {   
            PerformAllSetup();
        }

        private async void PerformAllSetup()
        {
            var unityServiceSetup = PerformUnityServiceSetup();
            await unityServiceSetup;
            
            var adsSetup = SetupAds();
            await adsSetup;
            
            var adLoad = LoadInterstitialAd();
            await adLoad;
            
            LoadScene(sceneToLoad);
        }
        
        private async Task PerformUnityServiceSetup()
        {
            await UnityServices.InitializeAsync();
            
            //TODO: We should allow proper sign ins : https://docs.unity.com/ugs/manual/authentication/manual/sdk-integration-intro
            
            //If player is not signed in
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        
        private async Task SetupAds()
        {
            MobileAds.Initialize(initStatus =>
            {
                adsLoaded = true;
            });
            
            while (!adsLoaded)
            {
                await Task.Yield();
            }
        }
        
        private async Task LoadInterstitialAd()
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

/*
    Data saving example:

    var data = new Dictionary<string, object>{ { "MySaveKey", "HelloWorld" } };
    await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    
    Documentation: https://docs.unity.com/ugs/en-us/manual/cloud-save/manual
*/
