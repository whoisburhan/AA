using System.Collections;
using UnityEngine;

public class AndroidShare : MonoBehaviour {
    [TextArea] public string m_Message;
    private string m_AppUrl;

    private void Start()
    {
        m_AppUrl = "https://play.google.com/store/apps/details?id=" + Application.identifier;
    }

    private bool shareProcessRunning = false;
    public void OnAndroidShareButtonClicked () {
        // GameAnalytics.LogEvent("lobby_share_btn_clicked");
        if (!shareProcessRunning) {
            StartCoroutine (ShareAndroidText ());
        }
    }

    IEnumerator ShareAndroidText () {
        shareProcessRunning = true;
        yield return new WaitForEndOfFrame ();

        if (Application.platform == RuntimePlatform.Android)
        {
         
            
            AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
            intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
            intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), m_Message);
            intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), m_AppUrl);

            //get the current activity
            AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
            //start the activity by sending the intent data
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject> ("createChooser", intentObject, "Share Via");
            currentActivity.Call ("startActivity", jChooser);
        } else {
            Application.OpenURL (m_AppUrl);
        }

        shareProcessRunning = false;
    }
}