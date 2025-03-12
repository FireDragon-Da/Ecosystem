using UnityEngine;

public class Astro : MonoBehaviour
{
    private AstroState currentState = AstroState.Floating;

    private Vector2 target;
    float placetostop = 0;
    public float Astrospeed;
    public enum AstroState{
        Floating,
        Standing,
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
        UpdateState();
    }



   
    public void StartState(AstroState newState)
    {
        EndState(currentState);
        switch(newState)
        {
            case AstroState.Standing:

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
                case AstroState.Standing:


                    break;

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
}


