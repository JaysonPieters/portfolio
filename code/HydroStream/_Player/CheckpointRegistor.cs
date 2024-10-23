using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRegistor : MonoBehaviour
{
    [SerializeField] private CheckpointID ID;
    public int currentCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            ID = other.GetComponent<CheckpointID>();
            if (ID.checkpointID == currentCheckpoint + 1)
            {
                currentCheckpoint = ID.checkpointID;
            }
        }
    }
}
