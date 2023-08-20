using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleMaster.Mainmenu
{
    public class QuitPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private Button m_yes;

        private void Start()
        {
            m_yes.onClick.AddListener(Quit);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) 
            {
                m_CanvasGroup.alpha = 1f;
                m_CanvasGroup.blocksRaycasts = true;
                m_CanvasGroup.interactable = true;
            }
        }

        private void Quit() 
        {
            Application.Quit();
        }
    }
}