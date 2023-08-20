using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameOptions
{
    public int frequency;
    public int timer;
    public string game;
    public string key;
    public string docDomain;
    public string baseName;
    public int gameVolume;
}

public class WeeGooAdManager : MonoBehaviour
{
    private static WeeGooAdManager _instance;

    public static WeeGooAdManager Instance { get { return _instance; } private set { } }

    /*
    public static WeeGooAdManager Instance{
        get
        {
            if (_instance == null)
            {
                _instance = (WeeGooAdManager)FindObjectOfType(typeof(WeeGooAdManager));
                

                if (FindObjectsOfType(typeof(WeeGooAdManager)).Length > 1)
                {

                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<WeeGooAdManager>();
                    //singleton.name = typeof(WeeGooAdManager).ToString();
                    singleton.name = "WeeGooAdsManager";

                    DontDestroyOnLoad(singleton);

                }
            }

            return _instance;
        }

        private set { }
    }
    */


    private void Awake()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (pingCount == 0)
            {
                try
                {
                    Debug.Log("Initing weegooevoads 2 ...");                
                    Ping(Instance.name.ToString());
                    
                    pingCount++;

                    if (OnReady != null)
                    {                    
                        RegisterRewardCallbacks();                      
                    }  

                }
                catch (System.IO.IOException)
                {
                    Debug.Log("Awake: Ping only available in live environment.");
                }
            }
        #else 
            Debug.LogWarning("WGSDK: Ads will only work in browser, please build your game and upload it to a server." );
        #endif

    }


    /*
    private void Awake() {

        Ping(Instance.name.ToString());
    }
    */


    private int adsCounter = 0;

    [Header("Automatically triggered ads", order = 2)]
    [Tooltip("This should always 0. Only in case you wish to automatically show an ad on time based interval. If that is teh case please specify here the number of seconds.")]
    public int AdInterval = 0;

    [Header("How often to show the ad?", order = 3)]
    [Tooltip("This field reffers to how many ad calls should be made before actually showing an ad. Ex: If the value is 2, the ad will be displayed every second ad call.")]
    public int ShowAdEveryNEvent = 1;


    private bool isShowing = false;
    private float initialVolume = 0f;
    private float timeSinceStart = 0f;
    private int pingCount = 0;
    private string homeBase;
    int seconds;
    private bool locked = true;
    private bool canIncrement = true;

    private readonly AudioListener Sounds;

    private bool SoundHasBeedPauseByAd = false;


    /* REWARD */

    private static bool RewardIsReadyToDisplay = false;

    [Header("RWARD AD OPTIONS", order = 4)]

    [Header("This function will be called when a reward ad is ready to be displayed.", order = 5)]
    public UnityEvent OnReady;

    [Header("This function will be called when the user completed the reward ad.", order = 6)]
    public UnityEvent OnSuccess;

    [Header("This function will be called when the user canceled the reward ad.", order = 7)]
    public UnityEvent OnFail;

    public void OnReadyMethod()
    {
        //Debug.Log("From Unity: Reward is ready!");
        //Debug.Log(RewardIsReadyToDisplay);
        RewardIsReadyToDisplay = true;
        //Debug.Log(RewardIsReadyToDisplay);
        OnReady?.Invoke();
    }

    public void OnSuccessMethod()
    {
        ExitAdState();
        OnSuccess?.Invoke();
    }

    public void OnFailMethod()
    {
        ExitAdState();
        OnFail?.Invoke();
    }

    public void ShowRewardAd()
    {
        //Debug.Log("From Unity: Show reward");
        //Debug.Log(RewardIsReadyToDisplay);
        if (RewardIsReadyToDisplay == true )
        {
            //Pause();
            //Debug.Log("From Unity: Showing reward");
            ShowRewardAdCallback();
            RewardIsReadyToDisplay = false;
        }else{
            //Debug.Log("From Unity: Can not show reward, not ready to display");
        }
    }

    [DllImport("__Internal")] private static extern void RegisterRewardCallbacks();
    [DllImport("__Internal")] private static extern void ShowRewardAdCallback();




    [DllImport("__Internal")] private static extern void Log(string s);

    [DllImport("__Internal")] private static extern void FetchAd();
    
    [DllImport("__Internal")] private static extern void RefetchReward();

    [DllImport("__Internal")] private static extern void Ping(string instanceName);

    //[DllImport("__Internal")] private static extern void RegisterRewardCallbacks(UnityEvent OnReady, UnityEvent OnFail, UnityEvent OnSuccess );

    public void FetchNewReward() {
        RefetchReward();
    }


    public void Unlock(string optionsFromFile)
    {

        //Log("Unlocking with: ");

        string tempOptions = Uri.UnescapeDataString(optionsFromFile);

        //options = JsonUtility.FromJson<GameOptions>( tempOptions );
        GameOptions myOptions = JsonUtility.FromJson<GameOptions>(tempOptions);

        if (myOptions.key == "_246_")
        {
            locked = false;
            Instance.locked = false;

            if ((int)myOptions.frequency > 0)
            {
                ShowAdEveryNEvent = (int)myOptions.frequency;
                Instance.ShowAdEveryNEvent = (int)myOptions.frequency;
            }

            if ((int)myOptions.timer > -1)
            {
                AdInterval = (int)myOptions.timer;
                Instance.AdInterval = (int)myOptions.timer;
            }

            homeBase = myOptions.docDomain;
            Instance.homeBase = myOptions.docDomain;

            Log("Frequency : " + ShowAdEveryNEvent);
            Log("AdInterval : " + AdInterval);
            Log("HomeBase : " + homeBase);
            Log("TempOptions : " + tempOptions);
            Log("Key : " + myOptions.key);


            //options = myOptions;


            CheckIfAllowed();

        }

    }

    public void GetAd()
    {
        Debug.Log("Getting ad: Locked? " + locked + "! / " + adsCounter + " / " + ShowAdEveryNEvent + " / " + Instance.name + " / " + Instance.locked);
        //if (locked == false)
        // {
        adsCounter++;
        
        if (adsCounter > 0  && adsCounter % ShowAdEveryNEvent == 0 )
        {
            EnterAdState();
        }
        //}

    }

    public bool IsLocked
    {
        get { return locked; }
    }

    public void Resume()
    {
        ExitAdState();
    }


    public void Pause()
    {
        isShowing = true;
      
        //pauza joc
        Time.timeScale = 0;

        //mute all sound
        Log("Unity: Muting sounds, volume is " + AudioListener.volume + " and initialVolume is " + initialVolume);

        if (AudioListener.volume > 0)
        {
            //initialVolume = AudioListener.volume;
        }

        //AudioListener.volume = 0f;
        AudioListener.pause = true;

        SoundHasBeedPauseByAd = true;

        Log("Unity: Pausing game, volume is now " + AudioListener.volume + " and initialVolume is " + initialVolume );


    }


    private void EnterAdState()
    {
        //Pause();
        FetchAd();
        seconds = 0;
    }

    private void ExitAdState()
    {

        isShowing = false;
        //resume game
        Time.timeScale = 1;
        //sound on

        if (SoundHasBeedPauseByAd == true)
        {
            Log("Unity: SoundHasBeedPauseByAd:" + SoundHasBeedPauseByAd );

            //AudioListener.volume = initialVolume;
            SoundHasBeedPauseByAd = false;
        }

        AudioListener.pause = false;

        canIncrement = true;

        Log( "Unity: Resuming game, volume is now " + AudioListener.volume );


    }


    void Start()
    {

    }


    public void CheckIfAllowed()
    {

        //Log("Domain should be: " + homeBase);

        string[] hosts = { homeBase, "bG9jYWxob3N0Og==", "d2dwbGF5ZXIuY29t" };
        bool allowed = false;
        for (int i = 0; i < hosts.Length; i++)
        {

            string host;
            if (i > 0)
            {
                byte[] decodedBytes = Convert.FromBase64String(hosts[i]);
                host = Encoding.UTF8.GetString(decodedBytes);
            }
            else
            {
                host = hosts[i];
            }

            string liveHost = Application.absoluteURL;

            string[] splitString = liveHost.Split(new string[] { "//" }, StringSplitOptions.None);
            liveHost = splitString[1];

            liveHost.Replace("www.", "");
            if (liveHost.Length > host.Length) liveHost = liveHost.Substring(0, host.Length);

            //Debug.Log( "Dom compare: " + host + " / " + liveHost );

            if (host == liveHost)
            {
                allowed = true;
                break;
            }
        }
        if (!allowed)
        {
            Application.OpenURL("http://" + homeBase);
        }

    }

    public void Init()
    {
        //Log(" ...Initing... ");
        if (pingCount == 0)
        {
            try
            {
                Debug.Log("Initing weegooevoads 1 ...");
#if UNITY_WEBGL && !UNITY_EDITOR
                Ping(Instance.name.ToString());
#endif
                pingCount++;

            }
            catch (System.IO.IOException)
            {
                Debug.Log("Ping only available in live environment.");
            }
        }
    }


    private int nextUpdate = 1;
    // Update is called once per frame
    void Update()
    {
        Init();

        if (locked == true)
            return;

        if (canIncrement == false)
            return;


        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

    }

    private int timesTo = 0;
    void UpdateEverySecond()
    {

        //Debug.Log( "Update by second" );
        timeSinceStart += Time.deltaTime;

        if (AdInterval > 0)
        {

            seconds += 1;

            if (adsCounter == 0)
            {
                timesTo = 1;
            }
            else
            {
                timesTo = adsCounter;
            }



            //Debug.Log((Mathf.Abs(seconds - (int)timesTo * AdInterval)) % AdInterval );
            if (!isShowing && seconds > 0 && seconds % AdInterval == 0 && (Mathf.Abs(seconds - (int)timesTo * AdInterval)) % AdInterval == 0)
            {
                canIncrement = false;
                adsCounter++;
                EnterAdState();
            }
        }
    }
}