using UnityEngine.Advertisements;

namespace Sufka.Ads
{
    public class AdsController
    {
        private const string ANDROID_ID = "4608227";
        private const string HINTS_AD_ID = "Hints";
        private const string BONUS_POINTS_AD_ID = "Hints";
        public string HintsAdId => HINTS_AD_ID;

        public  void Initialize(IUnityAdsListener listener)
        {
            Advertisement.AddListener(listener);
            Advertisement.Initialize(ANDROID_ID);
            // Advertisement.Initialize(ANDROID_ID, true);
        }

        public  void PlayHintAd()
        {
            Advertisement.Show(HINTS_AD_ID);
        }
    }
}
