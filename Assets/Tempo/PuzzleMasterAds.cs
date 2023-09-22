using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PuzzleMasterAds : MonoBehaviour
{
    [SerializeField] private Button tryItNowButton;

    private const string GAME_LINK = "https://play.google.com/store/apps/details?id=com.FantasyRealm.PuzzleMaster";
    // Start is called before the first frame update
    void Start()
    {
        tryItNowButton.onClick.AddListener(() =>
        {
            Application.OpenURL(GAME_LINK);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
