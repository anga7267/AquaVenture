using UnityEngine; using System.Collections; using System.Collections.Generic; 

public class GenerateObjects : MonoBehaviour{
    [Header("What objetc/s:")]
    public GameObject[] ObjectTemplate; // Hier Platzhalter für alle Objekte, die zufällig gesetzt werden. Man muss die Prefabs im Inspector draufziehen
    [Header("Where to generate:")]
    public Vector3 GenerationAreaFrom; public Vector3 GenerationAreaTo;    
    public bool RelativeToPlayer_XAxis = false; public bool RelativeToPlayer_YAxis = true; public bool RelativeToPlayer_ZAxis = false;
    
    [Header("(0 seconds between waves = only once)")]
    [Header("Time control:")]
    public int HowManyAtTheSameTime; 
    public float SecondsBetweenObjects;     
    public float SecondsBeforePlay; 
    public float SecondsBetweenWaves;
    
    [Header("Place to the ground:")]
    
    public float DeepToGround = 0.0F; 
    public float BuryDepth = 0.4F;
    [Header("Rotation:")]
    public bool RandomRotation = false; 
    public bool RandomRotationOnlyYAxis = false; 
    public bool RotationFromParentObject = false; 
    public Vector3 RotationFromObject;
    
    [Header("(Lifetime 0 means continuous)")]
    [Header("Further:")]
    public float LifetimeObject = 0.0F; 
    
    private GameObject generatedObject;
    
    void Start(){
        StartCoroutine (RandomLoop ());
    }

    IEnumerator RandomLoop(){
        yield return new WaitForSeconds (SecondsBeforePlay);
        while (true){
            for (int i = 0; i < HowManyAtTheSameTime; i++){
                int MyIndex = Random.Range(0, ObjectTemplate.Length); GameObject WhichPrefab = ObjectTemplate[MyIndex];

                Vector3 spawnPosition = new Vector3 (Random.Range (GenerationAreaFrom.x, GenerationAreaTo.x), Random.Range(GenerationAreaFrom.y, GenerationAreaTo.y), Random.Range(GenerationAreaFrom.z, GenerationAreaTo.z));
                if (RelativeToPlayer_XAxis==true || RelativeToPlayer_YAxis==true || RelativeToPlayer_ZAxis==true){
                    GameObject Spieler = GameObject.FindWithTag("Player");
                    if (Spieler!=null){ 
                        if (RelativeToPlayer_XAxis == true) { spawnPosition.x += Spieler.transform.position.x; }
                        if (RelativeToPlayer_YAxis == true) { spawnPosition.y += Spieler.transform.position.y; }
                        if (RelativeToPlayer_ZAxis == true) { spawnPosition.z += Spieler.transform.position.z; }
                    }
                }

               
                Quaternion spawnRotation; spawnRotation = Quaternion.identity; 
                if (RandomRotation == true) { spawnRotation = Random.rotation; }
                if (RotationFromParentObject == true) { spawnRotation = transform.rotation; } 
                if (RandomRotationOnlyYAxis == true) { 
                    Vector3 rotationVector = new Vector3(0, Random.Range(-90.0F, 90.0F), 0); spawnRotation = Quaternion.Euler(rotationVector); }
                if (RotationFromObject!=Vector3.zero) { spawnRotation = Quaternion.Euler(RotationFromObject); }
                    
                if (DeepToGround != 0.0F) { float YCorrection = - BuryDepth - GroundY(spawnPosition); spawnPosition += new Vector3(0, YCorrection, 0); }
                generatedObject = Instantiate(WhichPrefab, spawnPosition, spawnRotation);
                if (LifetimeObject != 0) { Destroy (generatedObject, LifetimeObject); }
                yield return new WaitForSeconds (SecondsBetweenObjects);
            }
            if (SecondsBetweenWaves == 0) { yield break; }
            yield return new WaitForSeconds (SecondsBetweenWaves);
        }
    }
        float GroundY(Vector3 start){
        Vector3 s1=start; float t = DeepToGround;
        RaycastHit hit; Ray downRay = new Ray(s1, Vector3.down);
        if (Physics.Raycast(downRay, out hit)){
            t = hit.distance; 
        }
        return t;
    }
}
    