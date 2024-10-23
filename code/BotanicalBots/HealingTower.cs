using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTower : MonoBehaviour
{
    private BuildingManager buildingManager;
    private CapsuleCollider capsuleCollider;

    //[SerializeField] private Animator anim;

    [SerializeField] public float healing = 1f;
    [SerializeField] public float timeUntilDestroyed = 2f;
    [SerializeField] public float Radius = 6.5f;
    [SerializeField] public float timeTillPulse = 0;
    PlayerManager player;

    private float timer;
    public bool isPlaced;
    bool isInRadius;

    private AudioSource audioSource;
    [SerializeField] private AudioClip growingSound;
    private bool playSound = true;

    [SerializeField] private GameObject placeText;

    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
        player = FindObjectOfType<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.radius = Radius;
        capsuleCollider.enabled = false;

        /*if (anim != null)
        {
            audioSource.PlayOneShot(growingSound);
            anim.Play("ThornTrap");
        }*/
    }

    private void Update()
    {
        if (isPlaced)
        {
            capsuleCollider.enabled = true;
            timer += Time.deltaTime;

            placeText.SetActive(false);
        }

        if (timer >= timeUntilDestroyed)
        {

            if (playSound)
            {
                PlaySound();
            }

            //anim.Play("DestroyTrap");
            Destroy(gameObject);
        }
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(growingSound);
        playSound = false;
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRadius = true;
            StartCoroutine(heal());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRadius = false;
        }
    }

    IEnumerator heal()
    {
        while (isInRadius)
        {
            player.currentHealth += healing;
            yield return new WaitForSeconds(timeTillPulse);
        }
        isInRadius = false;
    }
}
