using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausemenu;
    [SerializeField] private AudioClip uiSound;

    private AudioSource audioSource;

    private bool paused = false;
    public bool ifCanEscape = true;
    public GameObject slider;
    public GameObject slider2;

    public GameObject text;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pausemenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ifCanEscape)
            {
                if (paused)
                {
                    Pause();
                }
                else
                {
                    Unpause();
                }
            }
            
        }
    }

    void Pause()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        Time.timeScale = 1.0f;
        paused = false;
        pausemenu.SetActive(false);

        slider.SetActive(true);
        slider2.SetActive(true);
        text.SetActive(true);
    }

    void Unpause()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        paused = true;
        pausemenu.SetActive(true);
        Time.timeScale = 0.0f;

        slider.SetActive(false);
        slider2.SetActive(false);
        text.SetActive(false);
    }

    public void Resume()
    {
        audioSource.PlayOneShot(uiSound, 1.0f);
        Time.timeScale = 1.0f;
        paused = false;
        pausemenu.SetActive(false);

        slider.SetActive(true);
        slider2.SetActive(true);
        text.SetActive(true);
    }
}