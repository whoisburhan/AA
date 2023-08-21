using UnityEngine;
using UnityEngine.UI;

namespace GS.AA
{
    public class Sarah : MonoBehaviour
    {
        public Text DialogueText;
        [SerializeField] private GameObject sarahsBackground;

        private void OnEnable()
        {
            sarahsBackground.SetActive(true);
            GameManager.Instance.IsPlay = false;
            if (GameManager.Instance.requestForAppReview)
            {
#if UNITY_ANDROID || UNITY_IOS
                //StartCoroutine(InAppReviewManager.Instance.RequestReview());
#endif
            }
        }

        private void OnDisable()
        {
            sarahsBackground.SetActive(false);
            GameManager.Instance.IsPlay = true;
        }

        public void SetDialogue(string _dialogue = "")
        {
            DialogueText.text = _dialogue;
            GameManager.Instance.IsPlay = true;
        }

        public void DeactivateSarah()
        {
            this.gameObject.SetActive(false);
           

            if (GameManager.Instance.requestForAppReview)
            {
                GameManager.Instance.requestForAppReview = false;
            }
            else
            {
#if UNITY_ANDROID || UNITY_IOS
                //AdmobAds.instance.reqBannerAd();
#endif
            }
        }
    }
}
