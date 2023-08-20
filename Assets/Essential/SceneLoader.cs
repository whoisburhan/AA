using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private GameObject m_LoadingCanvas;
    [SerializeField] private Image m_ProgressBar;
    [SerializeField] private Text m_ProgressBarText;

    private float target;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("CURRENT_LEVEL", 0);
        //LoadScene("MainMenu");
        LoadScene("MainLevel");
    }

    private void Update()
    {
        if (target > 0f)
        {
            m_ProgressBar.fillAmount = Mathf.MoveTowards(m_ProgressBar.fillAmount, target, 3 * Time.deltaTime);
            m_ProgressBarText.text = (target * 100f).ToString("F2") + "%";
        }
    }

    public async void LoadScene(string sceneName) 
    {
        m_ProgressBar.fillAmount = 0;
        target = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        m_LoadingCanvas.SetActive(true);

        do 
        {
            await Task.Delay(100);
            target = scene.progress;

        }while (scene.progress < 0.9f);
        await Task.Delay(200);
        scene.allowSceneActivation = true;
        await Task.Delay(200);
        m_LoadingCanvas.SetActive(false);
    }
}
