using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    private Button playButton;

    private void Awake()
    {
        playButton = GetComponent<Button>();
    }

    private void Start()
    {
        playButton.onClick.AddListener(() => 
        {
            SceneLoader.Instance.LoadScene("MainLevel");
        });
    }
}
