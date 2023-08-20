using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace GS.AA
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static event Action OnGameReset;
        public static event Action OnGameFinished;
        public static event Action OnLevelCompleted;
        public static event Action OnGameOver;
        public static event Action<Color> OnColorSet;
        public static event Action<int> OnLevelStatusUpdate;

        [HideInInspector]
        public bool IsPlay = false;

        public int currentLevel = 0;
        public int LevelDataIndex;

        public int RequiredBall = 5;
        private int totalBall = 0;
        private int currentBallHitSoundIndex = 0;
        private bool isLevelCompletedOrGameOver = false;

        //Those variable are used when levels are randomly picked
        private bool isReloadedSameLevel = false;
        private int lastLevelColorIndex = 0;
        private int lastLevelIndex = 0;
        private int failedAttempt = 0;

        public float AdsTimeInterval = 120f;
        private float adsTimer;

        [HideInInspector] public bool requestForAppReview = false;

        [Header("Sarah")]
        [SerializeField] private GameObject sarah;

        [Header("Effects")]
        public GameObject rippleEffect;

        [Header("Ball Prefab")]
        public GameObject BallPrefab;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip gameOverClip;
        [SerializeField] private AudioClip victoryClip;
        [SerializeField] public AudioClip BallFireClip;
        [SerializeField] private AudioClip[] ballHitSounds;

        [Header("All Circles")]
        [SerializeField] private GameObject[] circles = new GameObject[3];

        [Header("All Spawners")]
        [SerializeField] public Spawner[] spawner = new Spawner[2];

        [Header("Color Array")]
        [SerializeField] private Color[] gameColors;

        [Header("Level Data")]
        [SerializeField] private List<LevelData> levelData;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Start()
        {
            adsTimer = AdsTimeInterval;

            currentLevel = PlayerPrefs.GetInt(TagsAndPlayerprefs.CURRENT_LEVEL, 0);
            isReloadedSameLevel = (PlayerPrefs.GetInt(TagsAndPlayerprefs.IS_LAST_LEVEL_RELOADED, 0) == 1) ? true : false;
            lastLevelIndex = PlayerPrefs.GetInt(TagsAndPlayerprefs.LAST_LEVEL_INDEX, 0);
            lastLevelColorIndex = PlayerPrefs.GetInt(TagsAndPlayerprefs.LAST_LEVEL_COLOR_INDEX, 0);
            requestForAppReview = false;

            try
            {
#if UNITY_ANDROID || UNITY_IOS
                if (currentLevel > 5)
                {
                    //AdmobAds.instance.requestInterstital();
                }
                //AdmobAds.instance.loadRewardVideo();
#endif
            }
            catch (Exception e) { }

            LoadLevel();
        }

        private void Update()
        {
            adsTimer -= Time.deltaTime;
        }

        public void LoadLevel()
        {

            Reset();
            OnGameReset?.Invoke();

            try
            {

#if UNITY_ANDROID || UNITY_IOS
                if (adsTimer < 0 && currentLevel > 5)
                {
                    //AdmobAds.instance.ShowInterstitialAd();
                    adsTimer = AdsTimeInterval;
                }

                if (currentLevel > 5)
                {
                    //AdmobAds.instance.reqBannerAd();
                }
#endif
            }
            catch (Exception e) { }

            Application.targetFrameRate = 30;


            if (requestForAppReview)
            {
                ActivateSarah("Do you like me? \n Rate me how much you like");
            }

            if (currentLevel < levelData.Count)
            {
                LevelDataIndex = currentLevel; //Need a function
            }
            else
            {
                if (!isReloadedSameLevel)
                {
                    LevelDataIndex = UnityEngine.Random.Range(0, levelData.Count);
                    lastLevelIndex = LevelDataIndex;
                    PlayerPrefs.SetInt(TagsAndPlayerprefs.LAST_LEVEL_INDEX, lastLevelIndex);
                }
                else
                {
                    LevelDataIndex = lastLevelIndex;
                }
            }

            LevelData _levelData = levelData[LevelDataIndex];

            RequiredBall = _levelData.NoOfBallSpawnInSpawner0 + _levelData.NoOfBallSpawnInSpawner1;
            spawner[0].SpawnedBallCounter = _levelData.NoOfBallSpawnInSpawner0;
            spawner[1].SpawnedBallCounter = _levelData.NoOfBallSpawnInSpawner1;
            spawner[0].SpawnBall();
            spawner[1].SpawnBall();

            #region Circle-0

            if (_levelData.ActiveCircle[0])
            {
                circles[0].SetActive(true);
                circles[0].GetComponent<Rotator>().RotateSpeed = _levelData.Circle_0_Speed;
                circles[0].GetComponent<Rotator>().IsRunAndStopMode = _levelData.Circle_0_IsRunAndStopMode;
                circles[0].GetComponent<Rotator>().IsRotateRight = _levelData.Circle_0_IsRotateRight;
                circles[0].GetComponent<Circle>().SetEnemyActivityStatus(_levelData.Circle_0_Active_Enemy);
                circles[0].GetComponent<Circle>().TwoSideRotation = _levelData.Circle_0_Two_Side_Rotation;
                circles[0].GetComponent<Circle>().levelText.text = (currentLevel + 1).ToString();
            }
            else
            {
                circles[0].SetActive(false);
            }

            #endregion

            #region Circle-1

            if (_levelData.ActiveCircle[1])
            {
                circles[1].SetActive(true);
                circles[1].GetComponent<Rotator>().RotateSpeed = _levelData.Circle_1_Speed;
                circles[1].GetComponent<Rotator>().IsRunAndStopMode = _levelData.Circle_1_IsRunAndStopMode;
                circles[1].GetComponent<Rotator>().IsRotateRight = _levelData.Circle_1_IsRotateRight;
                circles[1].GetComponent<Circle>().SetEnemyActivityStatus(_levelData.Circle_1_Active_Enemy);
                circles[1].GetComponent<Circle>().TwoSideRotation = _levelData.Circle_1_Two_Side_Rotation;
                circles[1].GetComponent<Circle>().levelText.text = (currentLevel + 1).ToString();
            }
            else
            {
                circles[1].SetActive(false);
            }

            #endregion

            #region Circle-2

            if (_levelData.ActiveCircle[2])
            {
                circles[2].SetActive(true);
                circles[2].GetComponent<Rotator>().RotateSpeed = _levelData.Circle_2_Speed;
                circles[2].GetComponent<Rotator>().IsRunAndStopMode = _levelData.Circle_2_IsRunAndStopMode;
                circles[2].GetComponent<Rotator>().IsRotateRight = _levelData.Circle_2_IsRotateRight;
                circles[2].GetComponent<Circle>().SetEnemyActivityStatus(_levelData.Circle_2_Active_Enemy);
                circles[2].GetComponent<Circle>().TwoSideRotation = _levelData.Circle_2_Two_Side_Rotation;
                circles[2].GetComponent<Circle>().levelText.text = (currentLevel + 1).ToString();
            }
            else
            {
                circles[2].SetActive(false);
            }

            #endregion

            if (currentLevel < levelData.Count)
            {
                OnColorSet?.Invoke(gameColors[currentLevel % gameColors.Length]);
                BallPrefab.GetComponent<ColorChanger>().SetColorInObject(gameColors[currentLevel % gameColors.Length]);
            }
            else
            {
                if (!isReloadedSameLevel)
                {
                    int _colorIndex = UnityEngine.Random.Range(0, gameColors.Length);
                    lastLevelColorIndex = _colorIndex;
                    PlayerPrefs.SetInt(TagsAndPlayerprefs.LAST_LEVEL_COLOR_INDEX, lastLevelColorIndex);
                    OnColorSet?.Invoke(gameColors[_colorIndex]);
                    BallPrefab.GetComponent<ColorChanger>().SetColorInObject(gameColors[_colorIndex]);
                }
                else
                {
                    OnColorSet?.Invoke(gameColors[lastLevelColorIndex]);
                    BallPrefab.GetComponent<ColorChanger>().SetColorInObject(gameColors[lastLevelColorIndex]);
                }
            }
            OnLevelStatusUpdate?.Invoke(currentLevel + 1);

            IsPlay = true;

            //ActivateSarah("Will you marry me");

            isReloadedSameLevel = true;
            PlayerPrefs.SetInt(TagsAndPlayerprefs.IS_LAST_LEVEL_RELOADED, 1);
        }

        public void AddBallInCount()
        {
            totalBall++;
            if (totalBall >= RequiredBall)
            {
                Debug.Log("ALL BALL HITTED!!");
                LevelComplete();
            }
            else
            {
                // Play different sound clip at each ball
                /*  if(currentBallHitSoundIndex >= ballHitSounds.Length)
                  {
                      currentBallHitSoundIndex = 0;
                  }
                  AudioManager.Instance.Play(ballHitSounds[currentBallHitSoundIndex]);
                  currentBallHitSoundIndex++; */
            }
        }

        public void GameOver()
        {
            if (!isLevelCompletedOrGameOver)
            {
                //Game Over Audio 
                AudioManager.Instance.Play(gameOverClip);

                // OnGameFinished?.Invoke();

                OnGameOver?.Invoke();
                isLevelCompletedOrGameOver = true;
                isReloadedSameLevel = true;


                failedAttempt++;
                if (failedAttempt == 4)
                {
                    ActivateSarah("Too Hard??\n you can skip level..");
                    failedAttempt = 0;
                }

                // Reset();
                // when the game is over
            }
        }

        public void LevelComplete()
        {
            if (!isLevelCompletedOrGameOver)
            {
                AudioManager.Instance.Play(victoryClip);

                currentLevel++;
                PlayerPrefs.SetInt(TagsAndPlayerprefs.CURRENT_LEVEL, currentLevel);


                if (currentLevel == 5 || currentLevel == 10)
                {
                    requestForAppReview = true;
                }

                failedAttempt = 0;
                OnLevelCompleted?.Invoke();
                isLevelCompletedOrGameOver = true;
                isReloadedSameLevel = false;
                PlayerPrefs.SetInt(TagsAndPlayerprefs.IS_LAST_LEVEL_RELOADED, 0);

                //OnGameFinished?.Invoke();
                // OnGameReset?.Invoke();
                // Reset();
                // when level is completed
            }


        }

        public void SkipLevel()
        {
            currentLevel++;
            PlayerPrefs.SetInt(TagsAndPlayerprefs.CURRENT_LEVEL, currentLevel);
            isReloadedSameLevel = false;
            failedAttempt = 0;
            PlayerPrefs.SetInt(TagsAndPlayerprefs.IS_LAST_LEVEL_RELOADED, 0);
            if (currentLevel == 5 || currentLevel == 10)
            {
                requestForAppReview = true;
            }

            adsTimer = AdsTimeInterval;
            LoadLevel();
        }

        // Enable Game Instuctor sara who help you to understand the game
        public void ActivateSarah(string _dialogue = "")
        {
            if (sarah.activeSelf == true)
            {
                sarah.SetActive(false);
            }
            Debug.Log("S");

            try
            {
#if UNITY_ANDROID || UNITY_IOS
                if (currentLevel > 5)
                {
                    //AdmobAds.instance.hideBanner();
                }
#endif
            }
            catch (Exception e) { };

            sarah.GetComponent<Sarah>().SetDialogue(_dialogue);
            sarah.SetActive(true);

            //  AdmobAds.instance.hideBanner();
        }


        private void Reset()
        {
            totalBall = 0;
            currentBallHitSoundIndex = 0;
            isLevelCompletedOrGameOver = false;
        }

    }
}
