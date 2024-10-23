using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornTrap : MonoBehaviour
{
    private BuildingManager buildingManager;
    private BoxCollider boxCollider;

    [SerializeField] private Animator anim;

    [SerializeField] public float speedReducer = 0.3f;
    [SerializeField] public float timeUntilDestroyed;
    [SerializeField] public int DoT = 10;

    private float timer;
    public bool isPlaced;

    private AudioSource audioSource;
    [SerializeField] private AudioClip growingSound;
    private bool playSound = true;

    [SerializeField] private GameObject placeText;

    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();

        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.enabled = false;

        if (anim != null)
        {
            audioSource.PlayOneShot(growingSound);
            anim.Play("ThornTrap");
        }
    }

    private void Update()
    {
        if (isPlaced == true)
        {
            boxCollider.enabled = true;
            timer += Time.deltaTime;

            placeText.SetActive(false);
        }

        if (timer >= timeUntilDestroyed)
        {
            boxCollider.center = boxCollider.center + new Vector3(0, 100, 100);

            if (playSound == true)
            {
                PlaySound();
            }

            anim.Play("DestroyTrap");
            Destroy(gameObject, 4);
        }
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(growingSound);
        playSound = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // All Enemies

            TrainingDummy trainingDummy = other.transform.GetComponent<TrainingDummy>();
            EnemyOne enemyOne = other.transform.GetComponent<EnemyOne>();
            MiniBoss miniBoss = other.transform.GetComponent<MiniBoss>();
            EndBoss endBoss = other.transform.GetComponent<EndBoss>();
            RapidBlast rapidBlast = other.transform.GetComponent<RapidBlast>();
            BlazeBot blazeBot = other.transform.GetComponent<BlazeBot>();
            if (trainingDummy != null)
            {
                trainingDummy.dummyAgent.speed *= speedReducer;
                trainingDummy.isInTrap = true;
                StartCoroutine(trainingDummy.DamageOverTime(DoT));
            }

            if (enemyOne != null)
            {
                enemyOne.walkingSpeed *= speedReducer;
                enemyOne.isInTrap = true;
                StartCoroutine(enemyOne.DamageOverTime(DoT));
            }

            if (miniBoss != null)
            {
                miniBoss.walkingSpeed *= speedReducer;
                miniBoss.isInTrap = true;
                StartCoroutine(miniBoss.DamageOverTime(DoT));
            }

            if (endBoss != null)
            {
                endBoss.movementSpeed *= speedReducer;
                endBoss.isInTrap = true;
                StartCoroutine(endBoss.DamageOverTime(DoT));
            }

            if (rapidBlast != null)
            {
                rapidBlast.movementSpeed *= speedReducer;
                rapidBlast.isInTrap = true;
                StartCoroutine(rapidBlast.DamageOverTime(DoT));
            }

            if (blazeBot != null)
            {
                blazeBot.movementSpeed *= speedReducer;
                blazeBot.isInTrap = true;
                StartCoroutine(blazeBot.DamageOverTime(DoT));
            }
    }

    private void OnTriggerExit(Collider other)
    {
        // All Enemies
        TrainingDummy trainingDummy = other.transform.GetComponent<TrainingDummy>();
        EnemyOne enemyOne = other.transform.GetComponent<EnemyOne>();
        MiniBoss miniBoss = other.transform.GetComponent<MiniBoss>();
        EndBoss endBoss = other.transform.GetComponent<EndBoss>();
        RapidBlast rapidBlast = other.transform.GetComponent<RapidBlast>();
        BlazeBot blazeBot = other.transform.GetComponent<BlazeBot>();

        if (trainingDummy != null)
        {
            trainingDummy.dummyAgent.speed /= speedReducer;

            trainingDummy.isInTrap = false;
        }
 
        if (enemyOne != null)
        {
            enemyOne.walkingSpeed /= speedReducer;

            enemyOne.isInTrap = false;
        }

        if (miniBoss != null)
        {
            miniBoss.walkingSpeed /= speedReducer;

            miniBoss.isInTrap = false;
        }

        if (endBoss != null)
        {
            endBoss.movementSpeed /= speedReducer;
            endBoss.isInTrap = false;
        }

        if (rapidBlast != null)
        {
            rapidBlast.movementSpeed /= speedReducer;
            rapidBlast.isInTrap = false;
        }

        if (blazeBot != null)
        {
            blazeBot.movementSpeed /= speedReducer;
            blazeBot.isInTrap = false;
        }
    }
}
