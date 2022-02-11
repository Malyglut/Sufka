using UnityEngine.Advertisements;

namespace Sufka.Ads
{
    public static class AdsController
    {
        private const string ANDROID_ID = "4608227";
        private const string HINTS_AD_ID = "Hints";
        private const string BONUS_POINTS_AD_ID = "Hints";

        public static void Initialize()
        {
            Advertisement.Initialize(ANDROID_ID);
            // Advertisement.Initialize(ANDROID_ID, true);
        }

        public static void ShowHintAd()
        {
            Advertisement.Show(HINTS_AD_ID);
        }
    }
}
