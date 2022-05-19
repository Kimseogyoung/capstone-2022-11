using UnityEngine;
using agora_gaming_rtc;

public class AgoraEngine : MonoBehaviour
{
    public string appID;
    public static IRtcEngine mRtcEngine;

    void Start()
    {
        if (mRtcEngine == null)
        {
            mRtcEngine = IRtcEngine.GetEngine(appID);
        }

        mRtcEngine.SetMultiChannelWant(true);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        if (mRtcEngine == null)
        {
            Debug.Log("engine is null");
            return;
        }

        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
    }

    private void OnApplicationQuit()
    {
        mRtcEngine?.LeaveChannel();
        mRtcEngine = null;
        IRtcEngine.Destroy();
    }
    private void OnDestroy()
    {
        if (mRtcEngine != null)
            mRtcEngine?.LeaveChannel();
    }
}
