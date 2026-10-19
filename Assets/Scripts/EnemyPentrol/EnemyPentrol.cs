using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyPentrol : MonoBehaviour
{
    [Space(5)]
    [Header("Assigns")]
    [SerializeField] private Transform player;
    
    [SerializeField] private Transform[] patrolPoints;

    [Space(10)]
    [Header("Layers")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask doorLayer;

    [Space(10)]
    public float speed = 5f;
    [SerializeField] private float chaseSpeed = 12f;

    [Space(10)]
    [Header("Ranges")]
    public float detectionRange = 5f;
    public float loseRange = 10f;

    public bool IsChasing => isChasing;

    private int currentPoint = 0;

    private bool goingToDoor = false;
    private bool isChasing = false;

    public AudioSource sound;

    private Rigidbody2D rb;
    private Door currentDoor;
    private Animator animator;
    private JumpScare jumpScare;
    public AudioSource jumpscaredSound;
    private float aiTimer = 0f;
    [SerializeField] private float aiUpdateInterval = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        sound.Stop();
    }

    void Start()
    {
        jumpScare = FindObjectOfType<JumpScare>();

        speed = Difficultys.chosenSpeed;
        chaseSpeed = Difficultys.chosenChaseSpeed;
        detectionRange = Difficultys.detectionRange;
        loseRange = Difficultys.loseRange;
    }
    

    void Update()
    {
        if (patrolPoints.Length == 0) return;

        aiTimer += Time.deltaTime;
        if (aiTimer >= aiUpdateInterval)
        {
            ChaseCheck();
            aiTimer = 0f;
        }

        EnemyMovesToDoors();
        EnemyMovement();
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            rb.MovePosition(rb.position + direction * chaseSpeed * Time.fixedDeltaTime);
        }
        
    }

    void EnemyMovesToDoors()
    {
        if (goingToDoor && currentDoor != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentDoor.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentDoor.transform.position) < 0.1f)
            {
                currentDoor.OpenDoor();
                goingToDoor = false;
            }

            return;
        }
    }

    // Add this new function anywhere inside your EnemyPentrol class
    public void ResetAIOnTeleport()
    {
        goingToDoor = false;
        currentDoor = null;
        
        // Instantly find and switch to the closest patrol point in the new room
        currentPoint = FindClosestPatrolPoint();
    }

    void EnemyMovement()
    {
        Transform target = patrolPoints[currentPoint];

        if (isChasing)
        {
            return;
        }

        if (CanMoveTo(target.position))
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // Wall detected - look for a door
            currentDoor = FindDoor(target.position);

            if (currentDoor != null)
            {
                goingToDoor = true;
            }
        }

        // Reached patrol point
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentPoint++;

            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }
        
        if (target.position.x > transform.position.x)
        {
            // Moving right
            transform.localScale = new Vector3(4, 4, 1);
        }
        else if (target.position.x < transform.position.x)
        {
            // Moving left
            transform.localScale = new Vector3(-4, 4, 1);
        }
    }

    void ChaseCheck()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distance <= detectionRange && CanSeePlayer())
        {
            isChasing = true;
            sound.Play();
        }

        if (isChasing && distance > loseRange)
        {
            isChasing = false;
            currentPoint = FindClosestPatrolPoint();
            sound.Stop();
        }
    }


    private bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position,obstacleLayer);

        return hit.collider == null;
    }

    private bool CanMoveTo(Vector2 target)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target, obstacleLayer);

        return hit.collider == null;
    }

    private Door FindDoor(Vector2 target)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target, obstacleLayer);

        if (hit.collider == null)
            return null;

        Collider2D[] doors = Physics2D.OverlapCircleAll(transform.position, 10f, doorLayer);

        Door closestDoor = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D doorCollider in doors)
        {
            Door door = doorCollider.GetComponent<Door>();

            if (door == null)
                continue;
 
            float distance = Vector2.Distance(transform.position, door.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDoor = door;
            }
        }

        return closestDoor;
    }

    private int FindClosestPatrolPoint()
    {
        float closestDistance = Mathf.Infinity;
        int closestPoint = 0;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, patrolPoints[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = i;
            }
        }

        return closestPoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.0f;
            jumpScare.flicker[0].gameObject.SetActive(true);
            jumpScare.flicker[1].gameObject.SetActive(true);
            jumpscaredSound.Play();

            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2f);

        jumpscaredSound.Stop();

        SceneManager.LoadScene("Mainmenu");

        jumpScare.flicker[1].gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }

    
}
