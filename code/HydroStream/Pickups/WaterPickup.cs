using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100;

    [SerializeField] int respawnTime = 15;
    float currentRespawnTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime / 3, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime / 2);
        if (gameObject.transform.position.y < -50)
        {
            currentRespawnTime += Time.deltaTime;
            if(currentRespawnTime >= respawnTime)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 100, gameObject.transform.position.z);
            }
        }
        else
        {
            currentRespawnTime = 0;
        }
    }
}
