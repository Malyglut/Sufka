using UnityEngine;
using UnityEngine.Advertisements;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace Sufka.Game.Ads
{
    public class AdsController
    {
        private const string ANDROID_ID = "4608227";
        private const string IOS_ID = "4608226";
        private const string HINTS_AD_ID = "Hints";
        private const string BONUS_POINTS_AD_ID = "Bonus_Points";
        public string HintsAdId => HINTS_AD_ID;
        public string BonusPointsAdId => BONUS_POINTS_AD_ID;

        public void Initialize(IUnityAdsListener listener)
        {
            Advertisement.AddListener(listener);

#if UNITY_ANDROID
            Advertisement.Initialize(ANDROID_ID);
#elif UNITY_IOS
            
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                {
                    ATTrackingStatusBinding.RequestAuthorizationTracking();
                }
            }
            
            Advertisement.Initialize(IOS_ID);
#endif
        }

        public void PlayHintAd()
        {
            Advertisement.Show(HINTS_AD_ID);
        }

        public void PlayBonusPointsAd()
        {
            Advertisement.Show(BONUS_POINTS_AD_ID);
        }
    }
}
