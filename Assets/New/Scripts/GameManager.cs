using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace FantasyRealm.AA
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;

		private bool gameHasEnded = false;

		public Rotator rotator;
		public Spawner spawner;

		public Animator animator;

		[Space]
		[Header("Aduio")]
		[SerializeField] private AudioSource audioSource;
		[Space]
		[SerializeField] private AudioClip victoryClip;
		[SerializeField] private AudioClip gameOverClip;
		[SerializeField] private AudioClip pinFireClip;
		[Header("Particles")]
		[SerializeField] private GameObject victoryParticle;

        private const string FILE_KEY = "CURRENT_LEVEL";

		private int pinLeftToCompleteLevel;

		public int PinLeftToCompleteLevel 
		{
			get { return pinLeftToCompleteLevel; }
			set 
			{
				pinLeftToCompleteLevel = value;
				//UIManager.Instance.UpdatePinLeftCounterInUI(value);
				if(pinLeftToCompleteLevel == 0) 
				{
					LevelCompleted();
				}

            }
		}

        public int CurrentLevel
        {
            get
            {
                return PlayerPrefs.GetInt(FILE_KEY, 0);
            }
            set
            {
                PlayerPrefs.SetInt(FILE_KEY, value);
            }
        }


        private void Awake()
        {
            if(Instance == null) 
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
			// Update Level In UI
			LevelGenerator.Instacne.RequestForLoadLevel(CurrentLevel);
        }

        public void EndGame()
		{
			if (gameHasEnded)
				return;

			audioSource.PlayOneShot(gameOverClip);

			rotator.enabled = false;
			spawner.enabled = false;

			animator.SetTrigger("EndGame");

			gameHasEnded = true;
		}


		public async void LevelCompleted() 
		{
            await Task.Delay(200);
            if (gameHasEnded)
                return;
            CurrentLevel++;            
            audioSource.PlayOneShot(victoryClip);
            victoryParticle.SetActive(true);
            UIManager.Instance.ActivateVictoryPanel();
            WeeGooAdManager.Instance.GetAd();

        }

		public void TriggerPinFireSound() 
		{
			audioSource.PlayOneShot(pinFireClip);
		}

		public void RestartLevel()
		{
			UIManager.Instance.ActivateGameOverPanel();
            WeeGooAdManager.Instance.GetAd();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

	}
}