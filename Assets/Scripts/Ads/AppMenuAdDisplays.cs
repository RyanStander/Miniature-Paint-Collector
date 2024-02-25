using System;
using UnityEngine;

namespace Ads
{
    public class AppMenuAdDisplays : MonoBehaviour
    {
        private float timeStamp;
        [SerializeField] private float adDisplayInterval = 60f;

        private void Start()
        {
            //load a banner ad and display it
            AdmobAdsScript.LoadBannerAd();
            timeStamp = Time.time + adDisplayInterval;
        }

        private void Update()
        {
            if (Time.time > timeStamp)
            {
                timeStamp = Time.time + adDisplayInterval;
                //load a banner ad and display it
                AdmobAdsScript.LoadBannerAd();
            }
        }
    }
}
