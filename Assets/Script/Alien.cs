using UnityEngine;

public class Alien : MonoBehaviour
{
    private AlienState currentState = AlienState.Searching;
    private Vector2 target;
    public float Alienspeed = 2f;
    private Rigidbody2D rigidbody2D;

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
        //spriteRenderer.enabled = true;
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
        if (spriteRenderer.enabled) return; // 如果 Sprite 还在，说明没必要复活

        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 5)
        {
            
            transform.position = GetRandomScreenPosition(); // 生成到随机位置
            spriteRenderer.enabled = true; // 重新显示
            transform.localScale = Vector3.zero; // 确保从小变大
            StartCoroutine(GrowEffect()); // 让 Alien 从小变大
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

    private Vector2 GetRandomScreenPosition()
    {
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(-4f, 4f);
        return new Vector2(x, y);
    }

    private System.Collections.IEnumerator GrowEffect()
    {
        float growTime = 0.5f; 
        float elapsed = 0f;
        while (elapsed < growTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsed / growTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

}
