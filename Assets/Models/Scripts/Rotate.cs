using UnityEngine; using System.Collections;

public class Rotate : MonoBehaviour{

	public float RotationToX=0.0F; public float RotationToY=10.0F; public float RotationToZ=0.0F;
	public float RandomRotation = 0.0F;

	void Start(){
		if (RandomRotation!=0.0f) {
            GetComponent<Rigidbody>().AddTorque(Random.Range(0, RandomRotation), Random.Range(0, RandomRotation), Random.Range(0, RandomRotation));}
		}

    void Update(){
        if (RotationToX!=0.0F || RotationToY!=0.0F || RotationToZ!=0.0F){
            transform.Rotate(new Vector3(RotationToX, RotationToY, RotationToZ) * Time.deltaTime);
        }
    }
}


