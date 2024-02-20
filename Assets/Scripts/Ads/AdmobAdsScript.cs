using System;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;

namespace Ads
{
    public class AdmobAdsScript : MonoBehaviour
    {
#if UNITY_ANDROID
        //private string bannerID = "ca-app-pub-1709328667164551/4901784331";//Real ID
        private string bannerID = "ca-app-pub-3940256099942544/6300978111";//Test ID
        //private string interstitialID = "ca-app-pub-1709328667164551/7046416153";//Real ID
        private string interstitialID = "ca-app-pub-3940256099942544/1033173712";//Test ID
#else
private string bannerID = "unexpected_platform";
#endif
        
        private BannerView bannerView;
        private InterstitialAd interstitialAd;

        [SerializeField] private TextMeshProUGUI outputLog;

        private void Start()
        {
            OutputLog("Using banner ID : " + bannerID);
            //MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(initStatus =>
            {
                OutputLog("Ads initialised!");
            });
        }

        #region Banner

        public void LoadBannerAd()
        {
            CreateBannerView();
            
            ListenToBannerEvents();
            OutputLog("Made it this far");
            if (bannerView == null)
            {
                CreateBannerView();
                OutputLog("Banner view is null. Creating a new one...");
            }

            var adRequest = new AdRequest();
            //adRequest.Keywords.Add("unity-admob-sample");
            
            OutputLog("Loading banner ad...");
            bannerView.LoadAd(adRequest);
        }

        private void CreateBannerView()
        {
            if (bannerView != null)
            {
                OutputLog("Destroying banner ad");
                DestroyBannerAd();
            }
            OutputLog("Creating banner ad");
            bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
            OutputLog("Banner ad created");
        }

        private void ListenToBannerEvents()
        {
            // Raised when an ad is loaded into the banner view.
            bannerView.OnBannerAdLoaded += () =>
            {
                OutputLog("Banner view loaded an ad with response : "
                          + bannerView.GetResponseInfo());
            };
            // Raised when an ad fails to load into the banner view.
            bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                OutputLog("Banner view failed to load an ad with error : "
                            + error);
            };
            // Raised when the ad is estimated to have earned money.
            bannerView.OnAdPaid += (AdValue adValue) =>
            {
                OutputLog(String.Format("Banner view paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            bannerView.OnAdImpressionRecorded += () =>
            {
                OutputLog("Banner view recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            bannerView.OnAdClicked += () =>
            {
                OutputLog("Banner view was clicked.");
            };
            // Raised when an ad opened full screen content.
            bannerView.OnAdFullScreenContentOpened += () =>
            {
                OutputLog("Banner view full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            bannerView.OnAdFullScreenContentClosed += () =>
            {
                OutputLog("Banner view full screen content closed.");
            };
            
            OutputLog("Listening to banner events");
        }

        public void DestroyBannerAd()
        {
            if (bannerView != null)
            {
                OutputLog("Destroying banner ad");
                bannerView.Destroy();
                bannerView = null;
            }
        }

        #endregion

        #region Interstitial

        public void LoadInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
            
            var adRequest = new AdRequest();
            //adRequest.Keywords.Add("unity-admob-sample");

            InterstitialAd.Load(interstitialID, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    OutputLog("Failed to load interstitial ad with error : " + error);
                }
                
                OutputLog("Interstitial ad loaded successfully"+ad.GetResponseInfo());

                interstitialAd = ad;
                InterstitialEvent(interstitialAd);

            });
        }

        public void ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                interstitialAd.Show();
            }
            else
            {
                OutputLog("Interstitial ad is not ready yet");
            }
        }

        private void InterstitialEvent(InterstitialAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                OutputLog(String.Format("Interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            interstitialAd.OnAdImpressionRecorded += () =>
            {
                OutputLog("Interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            interstitialAd.OnAdClicked += () =>
            {
                OutputLog("Interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                OutputLog("Interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                OutputLog("Interstitial ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                OutputLog("Interstitial ad failed to open full screen content " +
                          "with error : " + error);
            };
        }

        #endregion
        
        private void OutputLog(string log)
        {
            Debug.Log(log);
            //add log to the text, if it is then more than 10 lines, remove the first line
            var text = outputLog.text;
            text += log + "\n";
            if (text.Split('\n').Length > 10)
            {
                text = text.Substring(text.IndexOf('\n') + 1);
            }
            outputLog.text = text;
        }
    }
}
