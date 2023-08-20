using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleMaster.Mainmenu
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private Button m_PrivacyPolicyButton;
        [SerializeField] private Button m_CreditButton;
        [SerializeField] private Text m_VersionNoText;

        const string privacyPolicyURL = "https://docs.google.com/document/d/e/2PACX-1vSZ5oTeQ7wSdmlK4yolECLmleYxdKLax8OMgVTlLBzOzpseTt3EywCd8sTnt3S7HJ2pTLjgW2s_I7HL/pub";
        const string creditURL = "";

        private void Start()
        {
            m_VersionNoText.text = $"Version - {Application.version}";

            m_PrivacyPolicyButton.onClick.AddListener(() =>
            {
                Application.OpenURL(privacyPolicyURL);
            });

            m_CreditButton.onClick.AddListener(() => 
            {
                if (!string.IsNullOrWhiteSpace(creditURL))
                {
                    Application.OpenURL(creditURL);
                }
            });
        }
    }
}