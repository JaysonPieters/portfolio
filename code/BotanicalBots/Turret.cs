using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Turret : MonoBehaviour
{
    //Notes

    //loot at nearest enemy and just use the player pistol script without any spread.
    //ontriggerenter; check if the enterer is a enemy; if true then use a raycast to see if it can hit it; if true fire(); else do nothing;

    private AudioSource audioSource;

    [SerializeField] private AudioClip gunSound;
    [SerializeField] GameObject Bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    bool isFireing;
    public bool isPlaced;

    GameObject target = null;

    //Stats
    public float turretFirerate = 0.5f;
    public float turretLifetime = 10f;
    private float timer;

    [SerializeField] private GameObject placeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isFireing);

        if (target != null)
        {
            transform.LookAt(target.transform.position);
            StartCoroutine(fire());
        }

        if (isPlaced)
        {
            timer += Time.deltaTime;

            placeText.SetActive(false);
        }

        if (timer >= turretLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPlaced)
        {
            if (other.CompareTag("Enemy"))
            {
                if (Physics.Linecast(transform.position, other.transform.position, out RaycastHit hitInfo) && target == null)
                {
                    Debug.Log(hitInfo.collider.gameObject.name);
                    //Debug.Log("Blocked");
                    if (hitInfo.collider.CompareTag("Enemy"))
                    {
                        Debug.Log("Can fire at enemy");
                        target = other.gameObject;
                    }
                }
                else
                {
                    target = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
        catch(Exception e)
        {
            //Debug.LogWarning(e);
        }

    }



    IEnumerator fire()
    {
        if (!isFireing)
        {
            // Spawn bullet

            isFireing = true;
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.x = 90;


            // Spawn bullet
/*            float randomPitch = Random.Range(0.95f, 1.05f);

            audioSource.pitch = randomPitch;
            audioSource.PlayOneShot(gunSound);*/
            var rotation = Quaternion.Euler(rotationVector); //rotation stuffs
            Instantiate(Bullet, bulletSpawnPoint.position, rotation);
            yield return new WaitForSeconds(turretFirerate);
            isFireing = false;
        }
    }
}
