using UnityEngine.Advertisements;

namespace Sufka.Game.Ads
{
    public class AdsController
    {
        private const string ANDROID_ID = "4608227";
        private const string IOS_ID = "4608226";
        private const string HINTS_AD_ID = "Hints";
        private const string BONUS_POINTS_AD_ID = "Bonus_Points";
        
        private bool _initalized;
        
        public string HintsAdId => HINTS_AD_ID;
        public string BonusPointsAdId => BONUS_POINTS_AD_ID;

        public void Initialize(IUnityAdsListener listener)
        {
            if (_initalized)
            {
                return;
            }
            
            Advertisement.AddListener(listener);

#if UNITY_ANDROID
            Advertisement.Initialize(ANDROID_ID);
#elif UNITY_IOS
            Advertisement.Initialize(IOS_ID);
#endif

            _initalized = true;
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
