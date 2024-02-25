using System;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;

namespace Ads
{
    public static class AdmobAdsScript
    {
#if UNITY_ANDROID
        //private static string bannerID = "ca-app-pub-1709328667164551/4901784331";//Real ID
        private static string bannerID = "ca-app-pub-3940256099942544/6300978111";//Test ID
        //private static string interstitialID = "ca-app-pub-1709328667164551/7046416153";//Real ID
        private static string interstitialID = "ca-app-pub-3940256099942544/1033173712";//Test ID
#else
private string bannerID = "unexpected_platform";
#endif
        public static BannerView bannerView { get; private set; } 
        public static InterstitialAd interstitialAd { get; private set; }

        #region Banner

        public static void LoadBannerAd()
        {
            CreateBannerView();
            
            ListenToBannerEvents();
            if (bannerView == null)
            {
                CreateBannerView();
                Debug.Log("Banner view is null. Creating a new one...");
            }

            var adRequest = new AdRequest();
            //adRequest.Keywords.Add("unity-admob-sample");
            
            Debug.Log("Loading banner ad...");
            bannerView.LoadAd(adRequest);
        }

        private static void CreateBannerView()
        {
            if (bannerView != null)
            {
                DestroyBannerAd();
            }
            Debug.Log("Creating banner ad");
            bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
            Debug.Log("Banner ad created");
        }

        private static void ListenToBannerEvents()
        {
            // Raised when an ad is loaded into the banner view.
            bannerView.OnBannerAdLoaded += () =>
            {
                Debug.Log("Banner view loaded an ad with response : "
                          + bannerView.GetResponseInfo());
            };
            // Raised when an ad fails to load into the banner view.
            bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                Debug.Log("Banner view failed to load an ad with error : "
                          + error);
            };
            // Raised when the ad is estimated to have earned money.
            bannerView.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Banner view paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            bannerView.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Banner view recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            bannerView.OnAdClicked += () =>
            {
                Debug.Log("Banner view was clicked.");
            };
            // Raised when an ad opened full screen content.
            bannerView.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Banner view full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            bannerView.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Banner view full screen content closed.");
            };
            
            Debug.Log("Listening to banner events");
        }

        public static void DestroyBannerAd()
        {
            if (bannerView != null)
            {
                Debug.Log("Destroying banner ad");
                bannerView.Destroy();
                bannerView = null;
            }
        }

        #endregion

        #region Interstitial

        public static void LoadInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
            
            var adRequest = new AdRequest();

            InterstitialAd.Load(interstitialID, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log("Failed to load interstitial ad with error : " + error);
                }
                
                Debug.Log("Interstitial ad loaded successfully"+ad.GetResponseInfo());

                interstitialAd = ad;
                InterstitialEvent(interstitialAd);

            });
        }

        public static bool ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                interstitialAd.Show();
                return true;
            }
            else
            {
                Debug.Log("Interstitial ad is not ready yet");
                return false;
            }
        }
        
        public static void DestroyInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
        }

        private static void InterstitialEvent(InterstitialAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            interstitialAd.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            interstitialAd.OnAdClicked += () =>
            {
                Debug.Log("Interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.Log("Interstitial ad failed to open full screen content " +
                          "with error : " + error);
            };
        }

        #endregion
    }
}
