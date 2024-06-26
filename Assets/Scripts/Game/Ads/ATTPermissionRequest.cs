using UnityEngine;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace Sufka.Game.Ads
{
    public class ATTPermissionRequest : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_IOS
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                {
                    ATTrackingStatusBinding.RequestAuthorizationTracking();
                }
            }
#endif
        }
    }
}
