//using GS.PuzzleMaster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleMaster.Mainmenu
{
    public class RateUs : MonoBehaviour
    {
        [SerializeField] private Button m_RateButton;
        [SerializeField] private CanvasGroup m_CanvasGroup;

        private string m_AppUrl;

        private void Awake()
        {
            m_AppUrl = "https://play.google.com/store/apps/details?id=" + Application.identifier;
        }

        private void Start () 
        {
            m_RateButton.onClick.AddListener(Rate);

            //if (GameData.Instance.State.GameStates[1] == 10 || GameData.Instance.State.GameStates[1] == 25 || GameData.Instance.State.GameStates[1] == 50) 
            //{
            //    m_CanvasGroup.alpha = 1f;
            //    m_CanvasGroup.blocksRaycasts = true;
            //    m_CanvasGroup.interactable = true;
            //}
        }

        private void Rate() 
        {
            Application.OpenURL(m_AppUrl);
        }
    }
}