using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab; //gameobject to be created
    public float minTime = 2f;
    public float maxTime = 4f;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime)); //call function again
    }
        
}
