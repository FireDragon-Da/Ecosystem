using UnityEngine;

public class Alien : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AlienState currentState = AlienState.Searching;

    private Vector2 target;
    float placetostop = 0;
    public float Alienspeed;
    public GameObject Astro;
    public GameObject Pirate;
    private bool isFollowingPirate = false;
    private float followTimer = 0f; 
    private float respawnTimer = 0f;
    private bool isCollapsing = false; 
    public enum AlienState{
        Searching,
        Patrolling,
        Jumping
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartState(AlienState.Searching);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }



   
    public void StartState(AlienState newState)
    {
        EndState(currentState);
        switch(newState)
        {
            case AlienState.Patrolling:
                isCollapsing = true;
                respawnTimer = 5f; 
                break;

            case AlienState.Searching:

                Vector2 target = AlienTarget();
                Debug.Log("Activated");

                break;
        }
            currentState = newState;
    }



    public void UpdateState(){
  
        switch(currentState)
            {

                case AlienState.Searching:

                    AlienSearch();
                    break;
            }
    }
    public void EndState(AlienState oldState){
        switch(oldState){
        }
    }

    public Vector2 AlienTarget(){
        Debug.Log("Alien Targeting");
        float pos = Random.Range(0,3);
        if(pos == 0){
            float xcoord = Screen.width;
            float ycoord = Screen.height;
            return Camera.main.ScreenToWorldPoint(new Vector3(xcoord,ycoord,-1));
        }
        else if (pos == 1){
            float xcoord = Screen.width/2;
            float ycoord = Screen.height/2;
            return Camera.main.ScreenToWorldPoint(new Vector3(xcoord,ycoord,-1));
        }
        else if (pos == 2){
            float xcoord = 0;
            float ycoord = Screen.height/2;
            return Camera.main.ScreenToWorldPoint(new Vector3(xcoord,ycoord,-1));
        }
        else {
            float xcoord = Screen.width/2;
            float ycoord = 0;
            return Camera.main.ScreenToWorldPoint(new Vector3(xcoord,ycoord,-1));
        }
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
        
    }

    private void AlienSearch(){
        
        float TheDistance = Vector2.Distance(transform.position,target);
        if (TheDistance > placetostop ){
            transform.position = Vector2.MoveTowards(transform.position,target,Alienspeed);
            Debug.Log("Searching");
        }
        else{
            target = AlienTarget();
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
            StartState(AlienState.Searching);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
                    {
                        if (collision.gameObject == Astro) 
                        {

                            StartState(AlienState.Patrolling);
                        }
                        else if (collision.gameObject == Pirate)
                        {
                            isFollowingPirate = true;
                            followTimer = 5f; // Stop following after 5sec
                        }
                    }
}
