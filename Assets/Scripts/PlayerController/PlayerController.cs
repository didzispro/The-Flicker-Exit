using Unity.VisualScripting;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Space(5)]
    [SerializeField] private float speed = 10.0f;

    [Header("Assign")]
    [SerializeField] private GameObject keyObject;
    public GameObject crewBarObject;
    [SerializeField] private GameObject doorHandleObject;
    [SerializeField] private GameObject flashLightObject;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject toogleLight;

    [SerializeField] private GameObject keyDisplayText;
    [SerializeField] private GameObject crowbarDisplayText;
    [SerializeField] private GameObject doorHandleDisplayText;
    [SerializeField] private GameObject pressEDisplayText;
    [SerializeField] private GameObject requireKeyDisplayText;
    [SerializeField] private GameObject requireCrowbarDisplayText;
    [SerializeField] private GameObject requireDoorHandleDisplayText;

    [SerializeField] private CanvasGroup fadeRequireKeyDisplayText;
    [SerializeField] private CanvasGroup fadeRequireCrewBarDisplayText;
    [SerializeField] private CanvasGroup fadeRequireDoorHandleDisplayText;
    [SerializeField] private CanvasGroup fadeRequireFlashLightDisplayText;

    [SerializeField] private GameObject displayFlishLightText;

    private Coroutine keyCoroutine;
    private Coroutine crowbarCoroutine;
    private Coroutine doorHandleCoroutine;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private float fadeTime = 1f;


    [Space(10)]
    [Header("Checks")]
    public bool hasKey = false;
    public bool hasCrewBar = false;
    public bool hasDoorHandle = false;
    public bool hasFlashLight = false;

    private bool canPickUpKey = false;
    private bool canPickUpCrewBar = false;
    private bool canPickUpDoorHandle = false;
    private bool canPickUpFlashLight = false;
    private bool isHoldingItem = false;
    private bool isCurrentOff = false;
    private bool cantturnoff = false;
    public int crewBarCount = 0;

    private Vector2 direction;

    private Rigidbody2D rb;
    private Door door;
    private Animator animator;
    private Fide fide;
    private AudioSource audioSource;
    private CooldownSlider cooldownSlider;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        fide = FindObjectOfType<Fide>();
        audioSource = FindObjectOfType<AudioSource>();
        cooldownSlider = FindObjectOfType<CooldownSlider>();
    }

    void Update()
    {
        Movement();
        OpenDoor();
        CrewBar();
        DoorHandle();
        FlashLight();

        DropItems();

        ToogleLight();

        Running();
    }

    void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            cooldownSlider.ifRunning = true;
            cooldownSlider.start();
            speed = Difficultys.playerSpeed = 13f;
        }
        else
        {
            cooldownSlider.ifRunning = false;
            speed = Difficultys.playerSpeed = 10f;
        }
    }

    void ToogleLight()
    {
        if (cantturnoff) return;   
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (hasFlashLight)
            {
                if (!isCurrentOff)
                {
                    toogleLight.SetActive(true);
                    isCurrentOff = true;
                    cooldownSlider.ifOn = true;
                    cooldownSlider.start();
                    
                }
                else if (isCurrentOff)
                {
                    toogleLight.SetActive(false);
                    isCurrentOff = false;
                    cooldownSlider.ifOn = false;
                }
            }
        }

        if (cooldownSlider.slider.value <= 0)
        {
            toogleLight.SetActive(false);
            cantturnoff = true;
            cooldownSlider.ifOn = false;
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        direction = new Vector2(horizontal, vertical).normalized;
        

        bool isMoving = direction != Vector2.zero;

        animator.SetBool("IsRunning", isMoving);

        if (isMoving)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }


        if (hasFlashLight)
        {
            if (horizontal > 0)
            {
                // Right
                flashLight.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (horizontal < 0)
            {
                // Left
                flashLight.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (vertical > 0)
            {
                // Up
                flashLight.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else if (vertical < 0)
            {
                // Down
                flashLight.transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
        
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OpenDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (door != null && door.ifClose)
            {
                // Key
                if (door.requiresKey)
                {
                    if (hasKey && keyObject == door.requiredKey)
                    {
                        door.requiresKey = false;
                        door.unlocked = true;

                        keyObject.SetActive(false);
                        hasKey = false;
                        isHoldingItem = false;

                        keyDisplayText.SetActive(false);
                        requireKeyDisplayText.SetActive(false);

                        audioSource.PlayOneShot(doorSound, 1.0f);
                    }
                    else
                    {
                        if (keyCoroutine != null) StopCoroutine(keyCoroutine);

                        if (fadeRequireKeyDisplayText != null) fadeRequireKeyDisplayText.alpha = 1f;
                        
                        requireKeyDisplayText.SetActive(true);
                        keyCoroutine = StartCoroutine(WaitKey());
                        
                        
                    }
                }

                // Crew bar
                if (door.requiresCrewBar)
                {
                    if (hasCrewBar && crewBarObject == door.requiredCrewBar)
                    {
                        crewBarCount++;

                        door.requiresCrewBar = false;
                        door.crewBarUnlocked = true;

                        if (crewBarCount == 3)
                        {
                            crewBarObject.SetActive(false);
                            hasCrewBar = false;
                            isHoldingItem = false;

                            crowbarDisplayText.SetActive(false);
                            requireCrowbarDisplayText.SetActive(false);

                            audioSource.PlayOneShot(doorSound, 1.0f);
                        }
                    }
                    else
                    {
                        if (crowbarCoroutine != null) StopCoroutine(crowbarCoroutine);

                        if (fadeRequireCrewBarDisplayText != null) fadeRequireCrewBarDisplayText.alpha = 1f;
                        
                        requireCrowbarDisplayText.SetActive(true);
                        crowbarCoroutine = StartCoroutine(WaitCrowbar());
                    }
                }

                    // Door handle
                    if (door.requiresDoorHandle)
                    {
                        if (hasDoorHandle && doorHandleObject == door.requiredDoorHandle)
                        {
                            door.requiresDoorHandle = false;
                            door.doorHandleUnlocked = true;

                            doorHandleObject.SetActive(false);
                            hasDoorHandle = false;
                            isHoldingItem = false;

                            doorHandleDisplayText.SetActive(false);
                            requireDoorHandleDisplayText.SetActive(false);

                            audioSource.PlayOneShot(doorSound, 1.0f);
                            
                        }
                        else
                        {
                            if (doorHandleCoroutine != null) StopCoroutine(doorHandleCoroutine);

                            if (fadeRequireDoorHandleDisplayText != null) fadeRequireDoorHandleDisplayText.alpha = 1f;
                        
                            requireDoorHandleDisplayText.SetActive(true);
                            doorHandleCoroutine = StartCoroutine(WaitDoorHandle());
                        }
                    }
                    

                    // FINAL CHECK
                    if (!door.requiresKey && !door.requiresCrewBar && !door.requiresDoorHandle)
                    {
                        door.doorOpen = true;
                        audioSource.PlayOneShot(doorSound, 1.0f);
                    }
                }

            if (canPickUpKey && !isHoldingItem)
            {
                isHoldingItem = true;

                keyObject.transform.SetParent(holdPoint);
                keyObject.transform.localPosition = Vector3.zero;
                keyDisplayText.SetActive(true);
                
                hasKey = true;
            }
        }
    }

    void CrewBar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canPickUpCrewBar && !isHoldingItem)
            {
                isHoldingItem = true;

                crewBarObject.transform.SetParent(holdPoint);
                crewBarObject.transform.localPosition = Vector3.zero;
                    
                hasCrewBar = true;
                crowbarDisplayText.SetActive(true);
            }
        }
        
    }

    void DoorHandle()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canPickUpDoorHandle && !isHoldingItem)
            {
                isHoldingItem = true;

                doorHandleObject.transform.SetParent(holdPoint);
                doorHandleObject.transform.localPosition = Vector3.zero;

                doorHandleDisplayText.SetActive(true);
                hasDoorHandle = true;
            }
        }
    }

    void FlashLight()
    {
        if (canPickUpFlashLight)
        {
            flashLightObject.transform.SetParent(holdPoint);
            flashLightObject.transform.localPosition = Vector3.zero;

            displayFlishLightText.SetActive(true);
            hasFlashLight = true;

            StartCoroutine(WaitFlishLight());
        } 
    }

    void DropItems()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropKey();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCrewBar();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropDoorHandle();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            door = other.GetComponent<Door>();

            door.ifClose = true;
        }

        if (other.CompareTag("Key"))
        {
            canPickUpKey = true;
            pressEDisplayText.SetActive(true);
            keyObject = other.gameObject;
        }

        if (other.CompareTag("CrewBar"))
        {
            canPickUpCrewBar = true;
            pressEDisplayText.SetActive(true);
            crewBarObject = other.gameObject;
        }

        if (other.CompareTag("DoorHandle"))
        {
            canPickUpDoorHandle = true;
            pressEDisplayText.SetActive(true);
            doorHandleObject = other.gameObject;
        }

        if (other.CompareTag("FlashLight"))
        {
            canPickUpFlashLight = true;
            flashLightObject = other.gameObject;
        }

        if (other.CompareTag("Winning"))
        {
            fide.FadeIn();
            Time.timeScale = 0.0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            door.ifClose = false;
        }

        if (other.CompareTag("Key"))
        {
            canPickUpKey = false;
            pressEDisplayText.SetActive(false);
        }

        if (other.CompareTag("CrewBar"))
        {
            canPickUpCrewBar = false;
            pressEDisplayText.SetActive(false);
        }

        if (other.CompareTag("DoorHandle"))
        {
            canPickUpDoorHandle = false;
            pressEDisplayText.SetActive(false);
        }

        if (other.CompareTag("FlashLight"))
        {
            canPickUpFlashLight = false;
        }
    }

    void DropKey()
    {
        if (!hasKey)
            return;

        keyObject.transform.SetParent(null);
        keyObject.transform.position = transform.position;

        hasKey = false;
        isHoldingItem = false;
        keyDisplayText.SetActive(false);
    }

    void DropCrewBar()
    {
        if (!hasCrewBar)
            return;

        crewBarObject.transform.SetParent(null);
        crewBarObject.transform.position = transform.position;

        hasCrewBar = false;
        isHoldingItem = false;
        crowbarDisplayText.SetActive(false);
    }

    void DropDoorHandle()
    {
        if (!hasDoorHandle)
            return;

        doorHandleObject.transform.SetParent(null);
        doorHandleObject.transform.position = transform.position;

        hasDoorHandle = false;
        isHoldingItem = false;
        doorHandleDisplayText.SetActive(false);
    }

    IEnumerator WaitKey()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Fade(fadeRequireKeyDisplayText, 1.0f, 0.0f));
        requireKeyDisplayText.SetActive(false);
        keyCoroutine = null;
    }

    IEnumerator WaitCrowbar()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Fade(fadeRequireCrewBarDisplayText, 1.0f, 0.0f));
        requireCrowbarDisplayText.SetActive(false);
        crowbarCoroutine = null;
    }

    IEnumerator WaitDoorHandle()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Fade(fadeRequireDoorHandleDisplayText, 1.0f, 0.0f));
        requireDoorHandleDisplayText.SetActive(false);
        doorHandleCoroutine = null;
    }

    IEnumerator WaitFlishLight()
    {
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(Fade(fadeRequireFlashLightDisplayText, 1.0f, 0.0f));
        displayFlishLightText.SetActive(false);
    }



    IEnumerator Fade(CanvasGroup target, float start, float end)
    {
        float time = 0.0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            target.alpha = Mathf.Lerp(start, end, time / fadeTime);

            yield return null;

        }

        target.alpha = end;
    }
}
