using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyRealm.AA
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private Text m_PinLeftToHitText;
        [Space]
        [SerializeField] private GameObject m_GameOverPanel;
        [SerializeField] private GameObject m_GameOverPanelWithSkipOption;
        [SerializeField] private GameObject m_GameCompletePanel;
        [Space]
        [SerializeField] private Button m_GameOverPanelRetryButton;
        [SerializeField] private Button m_GameOverPanelBackButton;
        [Space]
        [SerializeField] private Button m_GameOverPanelWithSkipOptionRetryButton;
        [SerializeField] private Button m_GameOverPanelWithSkipOptionBackButton;
        [SerializeField] private Button m_GameOverPanelWithSkipOptionSkipButton;
        [Space]
        [SerializeField] private Button m_GameCompletePanelNextLevelButton;
        [SerializeField] private Button m_GameCompletePanelBackButton;
        [Space]
        [SerializeField] private Text m_GameOverPanelLevlNoText;
        [SerializeField] private Text m_GameOverPanelWithSpikOptionLevlNoText;
        [SerializeField] private Text m_GameCompletedPanelLevlNoText;

        private void Awake()
        {
            if (Instance == null) 
            {
                Instance = this;
            }
            else 
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            m_PinLeftToHitText.text = GameManager.Instance.CurrentLevel.ToString();
            m_GameCompletedPanelLevlNoText.text = "Level -" + GameManager.Instance.CurrentLevel.ToString();
            m_GameOverPanelLevlNoText.text = "Level -" + GameManager.Instance.CurrentLevel.ToString();
            m_GameOverPanelWithSpikOptionLevlNoText.text = "Level -" + GameManager.Instance.CurrentLevel.ToString();

            m_GameOverPanel.SetActive(false);
            m_GameCompletePanel.SetActive(false);
            m_GameOverPanelWithSkipOption.SetActive(false);

            InitButton();
        }

        private void InitButton() 
        {
            m_GameCompletePanelBackButton.onClick.AddListener(BackToMainMenu);
            m_GameOverPanelBackButton.onClick.AddListener(BackToMainMenu);
            m_GameOverPanelWithSkipOptionBackButton.onClick.AddListener(BackToMainMenu);

            m_GameCompletePanelNextLevelButton.onClick.AddListener(RestartLevel);
            m_GameOverPanelRetryButton.onClick.AddListener(RestartLevel);
            m_GameOverPanelWithSkipOptionRetryButton.onClick.AddListener(RestartLevel);

            m_GameOverPanelWithSkipOptionSkipButton.onClick.AddListener(() => { });
        }

        private void BackToMainMenu() 
        {
            SceneLoader.Instance.LoadScene("MainMenu");
        }

        private void RestartLevel()
        {
            SceneLoader.Instance.LoadScene("MainLevel");
        }

        public void UpdatePinLeftCounterInUI(int _no)
        {
            m_PinLeftToHitText.text = _no.ToString();
        }

        public void ActivateGameOverPanel() 
        {
            m_GameOverPanel.SetActive(true);
        }
        public void ActivateVictoryPanel()
        {
            m_GameCompletePanel.SetActive(true);

        }
        public void ActivateGameOverPanelWithSkipOption()
        {
            m_GameOverPanelWithSkipOption.SetActive(true);
        }
    }
}