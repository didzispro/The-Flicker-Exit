using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Space(5)]
    [Header("Assigns")]
    public GameObject requiredKey;
    public GameObject requiredCrewBar;
    public GameObject requiredDoorHandle;

    [Space(5)]
    [Header("Door Check")]
    public bool doorOpen = false;
    public bool ifClose = false;
    public bool unlocked = false;

    [Space(10)]
    [SerializeField] private Collider2D doorCollider;
    

    [Space(5)]
    [Header("Item Checks")]
    public bool crewBarUnlocked = false;
    public bool doorHandleUnlocked = false;
    public bool requiresKey;
    public bool requiresCrewBar;
    public bool requiresDoorHandle;

    private int objectsInside = 0;

    private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpen && ifClose)
        {
            bool isOpen = animator.GetBool("DoorOpen");
            animator.SetBool("DoorOpen", !isOpen);

            doorOpen = false;
            ifClose = false;
        } 
    }

    public void OpenDoor()
    {
        if (!doorOpen)
        {
            ifClose = false;

            animator.SetBool("DoorOpen", true);
            doorOpen = true;

            doorCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyPentrol>() != null || other.CompareTag("Player"))
        {
            EnemyPentrol enemy = other.GetComponent<EnemyPentrol>();

            if (enemy != null)
            {
                objectsInside++;

                if (!enemy.IsChasing)
                {
                    OpenDoor();
                }
                else
                {
                    if (requiredKey && requiredCrewBar)
                    {
                        if (unlocked && crewBarUnlocked)
                        {
                            OpenDoor();
                        }
                    }

                    else if (requiredKey)
                    {
                        if (unlocked)
                        {
                            OpenDoor();
                        }
                    }

                    else if (requiredCrewBar)
                    {
                        if (crewBarUnlocked)
                        {
                            OpenDoor();
                        }
                    }

                    else if (!requiredKey)
                    {
                        OpenDoor();
                    }
                    
                }              
            }
            
        }

    
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<EnemyPentrol>() != null)
        {
            objectsInside--;

            if (objectsInside <= 0)
            {
                StartCoroutine(Delay());
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);

        animator.SetBool("DoorOpen", false);

        doorOpen = false;

        doorCollider.enabled = true;

    }
}
