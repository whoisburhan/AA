using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace GS.AA
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public static event Action OnForceToDestroy;

        [Header("UI Panel")]
        [SerializeField] public GameObject gameOverPanel;
        [SerializeField] public GameObject levelCompletedPanel;
        [SerializeField] private Image gameOverPanelRivon;
        [SerializeField] private Image levelCompltedPanelRivon;

        [Header("UI Buttons")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button retryButton;
        [SerializeField] public Button skipButton;

        [Header("UI Text")]
        [SerializeField] private Text gameOverPanelLevelNoText;
        [SerializeField] private Text levelCompletePanelLevelNoText;

        [Header("Particles")]
        [SerializeField] private GameObject victoryParticle;


        private void OnEnable()
        {
            GameManager.OnLevelCompleted += ActivateLevelCompletedPanel;
            GameManager.OnGameOver += ActivatedGameOverPanel;
            GameManager.OnColorSet += RivonColorChanger;
            GameManager.OnLevelStatusUpdate += LevelNoUpdateUI;
            GameManager.OnGameReset += DeactivatePanel;
        }

        private void OnDisable()
        {
            GameManager.OnLevelCompleted -= ActivateLevelCompletedPanel;
            GameManager.OnGameOver -= ActivatedGameOverPanel;
            GameManager.OnColorSet -= RivonColorChanger;
            GameManager.OnLevelStatusUpdate -= LevelNoUpdateUI;
            GameManager.OnGameReset -= DeactivatePanel;
        }

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
            ButtonInitialization();
        }

        public void ActivatedGameOverPanel()
        {
            gameOverPanel.SetActive(true);
            if (GameManager.Instance != null && GameManager.Instance.currentLevel > 5)
            {
#if UNITY_ANDROID || UNITY_IOS
                //AdmobAds.instance.hideBanner();
#endif
            }
        }

        public void ActivateLevelCompletedPanel()
        {
            levelCompletedPanel.SetActive(true);
            victoryParticle.SetActive(true);
            if (GameManager.Instance != null && GameManager.Instance.currentLevel > 5)
            {
#if UNITY_ANDROID || UNITY_IOS
                //AdmobAds.instance.hideBanner();
#endif
            }
        }

        public void DeactivatePanel()
        {
            gameOverPanel.SetActive(false);
            levelCompletedPanel.SetActive(false);
            OnForceToDestroy?.Invoke();
        }

        public void RivonColorChanger(Color _color)
        {
            gameOverPanelRivon.color = _color;
            levelCompltedPanelRivon.color = _color;
        }

        private void ButtonInitialization()
        {
            nextButton.onClick.AddListener(()=> 
            {
                nextButton.transform.parent.gameObject.SetActive(false);
                OnForceToDestroy?.Invoke();
                GameManager.Instance.LoadLevel();
            });

            retryButton.onClick.AddListener(() => 
            {
                retryButton.transform.parent.gameObject.SetActive(false);
                OnForceToDestroy?.Invoke();
                GameManager.Instance.LoadLevel();
            });

            skipButton.onClick.AddListener(() => 
            {
//                OnForceToDestroy?.Invoke();
//                try{
//#if UNITY_ANDROID || UNITY_IOS
//               if (AdmobAds.instance.IsRewarededVideoLoaded())
//                {
//                    // Time.timeScale = 0f;
//                    AdmobAds.instance.showVideoAd();
//                   // UnityAdsManager.Instance.ShowAds("rewardedVideo");
//                }
//                else
//                {
//                    Time.timeScale = 1f;
//                    GameManager.Instance.ActivateSarah("Oops! No Rewarded\n ads available now...");
//                }
//#endif
//                }catch(Exception e)
//                {
//                    Time.timeScale = 1f;
//                    GameManager.Instance.ActivateSarah("Oops! No Rewarded\n ads available now...");
//                }
            });
        }


        public void LevelNoUpdateUI(int levelNo)
        {
            levelCompletePanelLevelNoText.text = "Level - " + levelNo.ToString();
            gameOverPanelLevelNoText.text = "Level - " + levelNo.ToString();
            victoryParticle.SetActive(false);
        }
    }
}