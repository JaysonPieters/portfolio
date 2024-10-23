using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private DeathScreen deathScreen;

    public bool infiniteHealthCheat;
    public int maxHealth;
    public float currentHealth;
    public int maxArmor;
    public float currentArmor;

    private AudioSource audioSource;
    private float damageReduction;

    [Header("Abilities")]
    public float thornTrapCooldown = 8f;
    public float thornTrapTimer;

    public float grenadeCooldown = 6f;
    public float grenadeTimer;

    public float healingTowerCooldown = 8f;
    public float healingTowerTimer;

    public float turretCooldown = 10f;
    public float turretTimer;

    public static bool canGunsFire = true;

    private void Awake()
    {
        LoadPlayer();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        deathScreen = FindObjectOfType<DeathScreen>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (infiniteHealthCheat) currentHealth = maxHealth;
        if (maxHealth < currentHealth) currentHealth = maxHealth;

        if (Input.GetKeyDown(KeyCode.C))
        {
            AddCoins(50);
        }

        LowEffect();
    }

    [SerializeField] private GameObject lowEffectUI;
    [SerializeField] private AudioClip heartBeat;

    private void LowEffect()
    {
        if (currentHealth < maxHealth / 3)
        {
            lowEffectUI.SetActive(true);
        }
        else
        {
            lowEffectUI.SetActive(false);
        }
    }

    public void ArmorOneActivated()
    {
        maxArmor = 40;
        damageReduction = 30;

        currentArmor = maxArmor;
    }

    public void DamageToPlayer(float damageToPlayer)
    {
        currentHealth = currentHealth - damageToPlayer / 100 * (100 - damageReduction);

        currentArmor = currentArmor - (damageToPlayer / 6f);

        FindObjectOfType<ScreenShake>().StartCoroutine(FindObjectOfType<ScreenShake>().Shaking());

        if (currentArmor <= 0)
        {
            currentArmor = 0;
            maxArmor = 0;
            damageReduction = 0;
        }

        if (currentHealth <= 0)
        {
            Dead();
            currentHealth = 0;
        }
    }

    public void AddCoins(int addedCoins)
    {
        int extraCoins = Random.Range(-10, 10);

        playerCoins = playerCoins + addedCoins + extraCoins;

        FindObjectOfType<CurrencyUI>().UpdateText(playerCoins);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    // External things to save
    public bool saveStarted = false;

    public bool pistolFound = false;
    public bool katanaFound = false;
    public bool shotgunFound = false;
    public bool assaultRifleFound = false;
    public bool minigunFound = false;
    public bool thornTrapFound = false;
    public bool healingTowerFound = false;
    public bool turretFound = false;
    public bool cutsceneOneDestroyed = false;
    public bool cutsceneTwoDestroyed = false;
    public bool receptionistDestroyed = false;
    public int playerCoins;

    // Level one
    public int objectiveIndex;
    public bool objectiveDone;

    // Level two
    public bool firstTimeSpawned;
    public int payloadCheckpointIndex;

    public bool bossTime;
 
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        saveStarted = data.saveStarted;

        // Saves the player health
        currentHealth = data.health;

        // Makes the player spawn at last saved location
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        // Saves the player his inventory
        pistolFound = data.pistol;
        katanaFound = data.katana;
        shotgunFound = data.shotgun;
        assaultRifleFound = data.ar;
        minigunFound = data.minigun;

        playerCoins = data.playerCoins;

        // Saves the player his abilitys
        thornTrapFound = data.thornTrap;
        healingTowerFound = data.healingTower;
        turretFound = data.turret;

        // Saves level aspects
        cutsceneOneDestroyed = data.cutsceneOne;
        cutsceneTwoDestroyed = data.cutsceneTwo;
        receptionistDestroyed = data.receptionist;

        // Level one
        objectiveIndex = data.objectiveIndex;

        // Level two
        firstTimeSpawned = data.firstTimeSpawned;
        payloadCheckpointIndex = data.payloadCheckpointIndex;
        bossTime = data.bossTime;
    }

    public void ResetPlayer()
    {
        Debug.Log("Reset Data");

        saveStarted = false;

        transform.position = new Vector3(-13.62f, 1.11f, -7.61f);
        pistolFound = false;
        katanaFound = false;
        shotgunFound = false;
        assaultRifleFound = false;
        minigunFound = false;
        thornTrapFound = false;
        healingTowerFound = false;
        turretFound = false;
        cutsceneOneDestroyed = false;
        cutsceneTwoDestroyed = false;
        receptionistDestroyed = false;
        objectiveIndex = 0;
        firstTimeSpawned = true;
        payloadCheckpointIndex = 0;
        bossTime = false;
        playerCoins = 0;
        SavePlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Dead()
    {
        //StartCoroutine(slowDownTime());
        Time.timeScale = 0.1f;
        deathScreen.ActivateDeathscreen();
        gameObject.SetActive(false);
    }

/*    IEnumerator slowDownTime()
    {
        while (Time.timeScale > 0.1f)
        {
            Time.timeScale -= Time.deltaTime;
            yield return new WaitForSeconds(0.05f);
        }
    }*/
}
