using UnityEngine;
using System.Collections;

public class Pirate : MonoBehaviour
{
    private PirateState currentState = PirateState.Wandering;
    private Vector2 target;
    public float pirateSpeed = 2.5f;
    public float escapeDistance = 3f; // minimum escape 

    public GameObject Astro; // Target
    
    private float respawnTimer = 0f;
    private SpriteRenderer spriteRenderer;

    public enum PirateState
    {
        Wandering,
        Escaping,
        Reviving
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartState(PirateState.Wandering);
    }

    void Update()
    {
        UpdateState();
    }

    public void StartState(PirateState newState)
    {
        EndState(currentState);

        switch (newState)
        {
            case PirateState.Wandering:
                target = GetRandomScreenPosition();
                break;

            case PirateState.Escaping:
                break;

            case PirateState.Reviving:
                spriteRenderer.enabled = false; // false
                respawnTimer = 5f; // revive
                break;
        }

        currentState = newState;
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case PirateState.Wandering:
                Wander();
                break;

            case PirateState.Escaping:
                Escape();
                break;

            case PirateState.Reviving:
                Revive();
                break;
        }
    }

    public void EndState(PirateState oldState)
    {
        if (oldState == PirateState.Escaping)
        {
        }
    }

    private Vector2 GetRandomScreenPosition()
    {
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(-4f, 4f);
        return new Vector2(x, y);
    }

    private void Wander()
    {
        if (Vector2.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, pirateSpeed * Time.deltaTime);
        }
        else
        {
            target = GetRandomScreenPosition();
        }

        if (Vector2.Distance(transform.position, Astro.transform.position) < escapeDistance)
        {
            StartState(PirateState.Escaping);
        }
    }

    private void Escape()
    {
        Vector2 directionAway = (transform.position - Astro.transform.position).normalized;
        Vector2 escapeTarget = (Vector2)transform.position + directionAway * escapeDistance;

        transform.position = Vector2.MoveTowards(transform.position, escapeTarget, pirateSpeed * Time.deltaTime);

        // Back to Wandering
        if (Vector2.Distance(transform.position, Astro.transform.position) > escapeDistance * 1.5f)
        {
            StartState(PirateState.Wandering);
        }
    }

    private void Revive()
    {
        if (spriteRenderer.enabled) return;

        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0)
        {
            Debug.Log("Pirate Respawning...");
            transform.position = Vector2.zero; // Center
            spriteRenderer.enabled = true;
            transform.localScale = Vector3.zero;
            StartCoroutine(GrowEffect());
            StartState(PirateState.Wandering);
        }
    }

    private IEnumerator GrowEffect()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Astro)
        {
            StartState(PirateState.Reviving);
        }
    }
}
