using UnityEngine;

public class Astro : MonoBehaviour
{
    private AstroState currentState = AstroState.Floating;

    private Vector2 target;
    float placetostop = 0;
    public float Astrospeed;
    public GameObject Alien;
    public GameObject Pirate;
    private bool isFollowingPirate = false;
    private float followTimer = 0f; 
    private float respawnTimer = 0f;
    private bool isCollapsing = false; 
    public enum AstroState{
        Floating,
        Collapsing,
        Jumping
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartState(AstroState.Floating);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPirate)
        {
            FollowPirate();
        
        }

        if (isCollapsing)
        {
            CollapseAndRespawn();
    
        }
        UpdateState();
    }



   
    public void StartState(AstroState newState)
    {
        EndState(currentState);
        switch(newState)
        {
            case AstroState.Collapsing:
                isCollapsing = true;
                respawnTimer = 5f; 
                break;

            case AstroState.Floating:

                Vector2 target = AstroTarget();
                Debug.Log("Activated");

                break;
        }
            currentState = newState;
    }



    public void UpdateState(){
  
        switch(currentState)
            {

                case AstroState.Floating:

                    AstroFloat();
                    break;
            }
    }
    public void EndState(AstroState oldState){
        switch(oldState){
        }
    }

    public Vector2 AstroTarget(){
        Debug.Log("Targeting");
        float xcoord = Random.Range(0,Screen.width);
        float ycoord = Random.Range(0,Screen.height);
        return Camera.main.ScreenToWorldPoint(new Vector3(xcoord,ycoord,0));
    }


private void CollapseAndRespawn()
    {
        if (transform.localScale.x > 0.1f)
        {
            transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0); // getting smaller
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false; // dissappear
            Debug.Log("Respawning");
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0.0f)
            {
                Debug.Log("Respawned");
                Respawn();
            }
        }
    }
    private void Respawn()
    { 
        if (Alien != null)
        {
            transform.position = (Vector2)Alien.transform.position + Vector2.down * 5f;
        }
        transform.localScale = Vector3.one; // back to normal scale
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        isCollapsing = false;
        StartState(AstroState.Floating);
    }

    private void AstroFloat(){
        
        float TheDistance = Vector2.Distance(transform.position,target);
        if (TheDistance > placetostop ){
            transform.position = Vector2.MoveTowards(transform.position,target,Astrospeed);
            Debug.Log("Floating");
        }
        else{
            target = AstroTarget();
        }
    }

    private void FollowPirate()
    {
        if (Pirate != null)
        {
            transform.position = Pirate.transform.position + new Vector3(-1, 0, 0); // Follow Pirate
        }

        followTimer -= Time.deltaTime;
        if (followTimer <= 0)
        {
            isFollowingPirate = false;
            StartState(AstroState.Floating);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
                    {
                        if (collision.gameObject == Alien) 
                        {

                            StartState(AstroState.Collapsing);
                        }
                        else if (collision.gameObject == Pirate)
                        {
                            isFollowingPirate = true;
                            followTimer = 5f; // Stop following after 5sec
                        }
                    }
}


