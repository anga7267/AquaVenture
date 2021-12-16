using UnityEngine; 

public class MoveZigZag : MonoBehaviour{
    
    public float Velocity =1.5F; public float HowFarX = 2.0F; public float HowFarZ = 2.0F;
    public float StopMovementAfter=0.0F; public float EndProgramAfter=0.0F;

    private float deltaX = 1.0F; private float deltaZ = 0.0F; private float startX; private float startZ;  private float moment;

    void Start(){
        startX=transform.position.x; startZ=transform.position.z; moment = Time.time;
    }

    void Update(){
        if (StopMovementAfter != 0 && (Time.time - moment > StopMovementAfter)) { Velocity=0; }
        if (EndProgramAfter != 0 && (Time.time - moment > EndProgramAfter)) { Application.Quit(); }

        float AdjustedSpeedX = Velocity * deltaX * Time.deltaTime;
        float AdjustedSpeedZ = Velocity * deltaZ * Time.deltaTime;
        transform.Translate(AdjustedSpeedX, 0.0F, AdjustedSpeedZ);
        if (deltaX==1.0F){
            if(transform.position.x >= startX + HowFarX) { startX = transform.position.x; deltaX=0.0F; deltaZ = 1.0F; }
        } else {
            if(transform.position.z >= startZ + HowFarZ) { startZ = transform.position.z; deltaZ=0.0F; deltaX = 1.0F; }
        }
    }
}
