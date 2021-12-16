using UnityEngine; 
public class Move_ZigZag_Wave : MonoBehaviour{

    [Header("ZigZag")]
    public Vector3 howlong1 = new Vector3(2.0F, 0.0F, 0.0F); 
    public Vector3 howlong2 = new Vector3(0.0F, 0.0F, 1.0F); 
    public float Velocity1 = 1.0F; public float Velocity2 = 0.5F;
    
    [Header("Wave (if >0, instead of ZigZag")]
    public Vector3 WaveTiming = Vector3.zero;
    [Header("Bounce")]
    public bool BounceBehaviour=false;
    [Header("General")]
    public float StopMovementAfter=0.0F; public float EndProgramAfter=0.0F;

    private Vector3 delta1, delta2, targetPos; private float moment; private bool Axis1Active = true;

    // Start is called before the first frame update
    void Start()    {
        delta1 = howlong1.normalized; delta2 = howlong2.normalized;
        targetPos= transform.position+howlong1; moment = Time.time;
    }

    // Update is called once per frame
    void Update()    {
        if (StopMovementAfter != 0.0F && (Time.time - moment > StopMovementAfter)) { return; }
        if (EndProgramAfter != 0.0F && (Time.time - moment > EndProgramAfter)) { Application.Quit(); }
      
        if (WaveTiming != Vector3.zero){
                transform.position += WaveTiming * Mathf.Sin(Time.time);
        }
        else
        {
            if (Axis1Active == true){
            transform.Translate(Velocity1 * delta1.x * Time.deltaTime, Velocity1 * delta1.y * Time.deltaTime, Velocity1 * delta1.z * Time.deltaTime);
            } else {
            transform.Translate(Velocity2 * delta2.x * Time.deltaTime, Velocity2 * delta2.y * Time.deltaTime, Velocity2 * delta2.z * Time.deltaTime);    
            }
            if (Vector3.Distance(transform.position, targetPos) < 0.1F){  
                if (Axis1Active==true) { targetPos = transform.position + howlong2; } else { targetPos = transform.position + howlong1; } 
                Axis1Active =! Axis1Active; 
            }
        }

    }
}
