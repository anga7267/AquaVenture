using UnityEngine; 

public class MoveStraight : MonoBehaviour{
    [Header("Either one object moves directly...")]
    public Vector3 Velocity = new Vector3(1.5F, 0.0F, 0.0F);
    
    [Header("... or with force (addcomp. rigidbody!)")] // ohne Rigidbody kann keine Kraft ausgeführt werden
    public Vector3 ForceVector = Vector3.zero; 

    [Header("Or")]
    public float RotateMotionTo = 0.0F; public float StopMovementAfter=0.0F; public float EndProgramAfter=0.0F;

    private float moment;
    void Start(){
        moment = Time.time; 
    }

    // Update is called once per frame
    void Update(){
        if (StopMovementAfter != 0 && (Time.time - moment > StopMovementAfter)) { Velocity = Vector3.zero; }
        if (EndProgramAfter != 0 && (Time.time - moment > EndProgramAfter)) { Application.Quit(); }
        if (RotateMotionTo != 0 && (Time.time - moment> RotateMotionTo)) { moment = Time.time; Velocity = (-1) * Velocity; }
        if (Velocity != Vector3.zero){ transform.Translate(Velocity * Time.deltaTime); } 
        else 
        { GetComponent<Rigidbody>().AddForce(ForceVector); }
    }
}
