using UnityEngine;

public class Alien : MonoBehaviour
{
    private AlienState currentState = AlienState.Searching;
    private Vector2 target;
    public float Alienspeed = 2f;

    public GameObject Astro;
    public GameObject Pirate;
    
    private float respawnTimer = 0f;
    private bool isFacingPirate = false;

    private SpriteRenderer spriteRenderer;

    public enum AlienState
    {
        Searching,
        Patrolling,
        Facing,
        Respawning
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartState(AlienState.Searching);
    }

    void Update()
    {
        UpdateState();
    }

    public void StartState(AlienState newState)
    {
        EndState(currentState);

        switch (newState)
        {
            case AlienState.Patrolling:
                target = SelectRandomCorner();
                break;

            case AlienState.Facing:
                isFacingPirate = true;
                transform.rotation = Quaternion.Euler(0, 0, 90); // knock down
                spriteRenderer.enabled = false; // disappear
                StartState(AlienState.Respawning);
                break;

            case AlienState.Respawning:
                if (spriteRenderer.enabled == false){
                    respawnTimer = 5f; // Respawn timer
                }
                break;
        }

        currentState = newState;
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case AlienState.Searching:
                AlienSearch();
                break;

            case AlienState.Patrolling:
                Patrol();
                break;

            case AlienState.Respawning:
                Respawn();
                break;
        }
    }

    public void EndState(AlienState oldState)
    {
        if (oldState == AlienState.Facing)
        {
            isFacingPirate = false;
            transform.rotation = Quaternion.identity; // back to normal angle
        }
    }

    private Vector2 SelectRandomCorner()
    {
        int choice = Random.Range(0, 4);
        float x = (choice % 2 == 0) ? 0 : Screen.width;
        float y = (choice < 2) ? Screen.height : 0;
        return Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
    }

    private void Patrol()
    {
        if (Vector2.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Alienspeed * Time.deltaTime);
        }
        else
        {
            target = SelectRandomCorner();
        }
    }

    private void AlienSearch()
    {
        if (Vector2.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Alienspeed * Time.deltaTime);
        }
        else
        {
            target = SelectRandomCorner();
        }
    }

    private void Respawn()
    {
        if (spriteRenderer.enabled) return; // Stop reviving

        respawnTimer -= Time.deltaTime;

        Debug.Log("Alien respawning");

        if (respawnTimer <= 0)
        {
            transform.position = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            spriteRenderer.enabled = true; // Visible
            StartState(AlienState.Searching);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Pirate)
        {
            StartState(AlienState.Facing);
        }
        else if (collision.gameObject == Astro)
        {
            StartState(AlienState.Patrolling);
        }
    }
}
