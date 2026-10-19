using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject settingsTabPanel;
    [SerializeField] private GameObject blackBackground;
    [SerializeField] private GameObject mainmenuPanel;
    [SerializeField] private CanvasGroup warningText;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private AudioClip uiSound;
    [SerializeField] private AudioClip warningSound;
    [SerializeField] private GameObject difficultyPanel;
    public GameObject pausemenu;

    public GameObject slider;
    public GameObject slider2;

    public GameObject text;
    private RandomSpawnItem randomSpawnItem;
    private PauseManager pauseManager;

    public AudioSource musicHorror;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pauseManager = FindObjectOfType<PauseManager>();
        randomSpawnItem = FindObjectOfType<RandomSpawnItem>();

        if (musicHorror != null)
        {
            musicHorror.Stop();
        }

        if (blackBackground != null)
        {
            blackBackground.SetActive(true);
        }

        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
        }

        if (mainmenuPanel != null)
        {
            mainmenuPanel.SetActive(false);
        }

        if (settingsTabPanel != null)
        {
            settingsTabPanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(warningSound, 1.0f);
        }

        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (blackBackground.activeInHierarchy && warningText.gameObject.activeInHierarchy)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        StartCoroutine(Winning());
    }

    public void PlayGame()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        
        difficultyPanel.SetActive(true);
        mainmenuPanel.SetActive(false);
    }

    public void MakeGame()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        
        SceneManager.LoadScene("MainGame");
    }

    public void Mainmenu()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Mainmenu");
    }

    public void Settings()
    {
        if (pauseManager != null)
        {
            pauseManager.ifCanEscape = false;
        }
        
        audioSource.PlayOneShot(uiSound, 1.0f);
        settingsPanel.SetActive(true);

        if (pausemenu != null)
        {
            pausemenu.SetActive(false);
        }

    }

    public void AudioPanel()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        audioPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        Application.Quit();
    }

    public void BackButtonAudio()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        audioPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackButtonDifficulty()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        difficultyPanel.SetActive(false);
        mainmenuPanel.SetActive(true);
    }


    public void BackMainmenu()
    {
        if (pauseManager != null)
        {
            pauseManager.ifCanEscape = true;
        }
        
        audioSource.PlayOneShot(uiSound, 1.0f);
        settingsPanel.SetActive(false);

        if (pausemenu != null)
        {
            pausemenu.SetActive(true);
        }
    }

    public void Controls()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        settingsTabPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void BackSettingsTab()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        settingsTabPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Winning()
    {
        yield return StartCoroutine(Fide(warningText, 1.0f, 0.0f));
    }

    IEnumerator Fide(CanvasGroup target, float start, float end)
    {
        float time = 0.0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            target.alpha = Mathf.Lerp(start, end, time / fadeTime);

            yield return null;

        }

        target.alpha = end;

        blackBackground.SetActive(false);
        warningText.gameObject.SetActive(false);
        mainmenuPanel.SetActive(true);
        musicHorror.Play();
    }
}
