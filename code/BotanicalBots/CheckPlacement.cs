using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    private BuildingManager buildingManager;

    [SerializeField] private GameObject crossIcon;

    private void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
        {
            crossIcon.SetActive(true);
            buildingManager.canPlace = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
        {
            crossIcon.SetActive(true);
            buildingManager.canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
        {
            crossIcon.SetActive(false);
            buildingManager.canPlace = true;
        }
    }
}
