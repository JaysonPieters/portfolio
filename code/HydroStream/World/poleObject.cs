using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poleObject : MonoBehaviour
{
    [SerializeField] float despawnTime = 10f;
    float currentDespawnTime;

    Rigidbody body;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.useGravity)
        {
            Destroy(gameObject, despawnTime);
        }
    }
}
