using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KnightBehavior : MonoBehaviour
{
    Vector2 movement;

    Rigidbody body;

    public static SoundFXController soundController;

    bool hurt;

    public static float speed = 4;
    public float baseSpeed = 4;
    public static int speedLevel = 0; //One level = +10%speed

    public static float maxHealth = 100;
    public static float health;
    public float baseMaxHealth = 100;
    public static int healthLevel = 0; //One level = +10%hp
    public static int protectionLevel = 0; //One level = +10%damage Reduction

    public static float mainDamage = 5;

    public static int luck = 10; //Lower is better

    public static int EXP;
    public static int LVL;
    public static float EXPToNextLVL = 100;

    [SerializeField] GameObject playerModel;
    //[SerializeField] GameObject attackSpawn;
    [SerializeField] GameObject attackArrow;

    //[SerializeField] GameObject swordAttack;

    public GameObject upgradeUI;
    [SerializeField] UpgradeButton[] upgradeButtons;

    DeathScreen deathUI;

    bool hasAttacked;
    bool autoAttack;

    Animator animator;

    Ray ray;
    Plane plane = new Plane(Vector3.down, 1.9f);

    // Start is called before the first frame update
    void Start()
    {
        soundController = FindObjectOfType<SoundFXController>();
        EXP = 0;
        EXPToNextLVL = 100;
        Time.timeScale = 1;
        deathUI = FindObjectOfType<DeathScreen>();
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody>();
        Timer.time = 0;
        updateStats();
        health = maxHealth;
        //Abilities.refreshUpgrades();
        
    }

    
    void levelUP()
    {
        EXPToNextLVL = (int) (EXPToNextLVL * 1.2f);
        LVL++;
        Debug.Log("Level UP");
        generateSkillUpgrades();
    }
    
    void generateSkillUpgrades()
    {
        upgradeUI.SetActive(true);
        int randomUpgrade;
        if(Abilities.avalibleUpgrades.Count == 0 )
        {
            Abilities.applyUpgrade("");
            Debug.Log("no upgrades to choose from");
        }
        else
        {
            Time.timeScale = 0;
            foreach (var upgradebutton in upgradeButtons)
            {
                if (Abilities.avalibleUpgrades.Count > 0)
                {
                    upgradebutton.gameObject.SetActive(true);
                    randomUpgrade = Random.Range(0, Abilities.avalibleUpgrades.Count);
                    upgradebutton.upgradeString = Abilities.avalibleUpgrades[randomUpgrade];
                    upgradebutton.GetComponent<Button>().interactable = false;
                    upgradebutton.updateText();
                    Abilities.usedUpgrades.Add(Abilities.avalibleUpgrades[randomUpgrade]);
                    Abilities.avalibleUpgrades.Remove(Abilities.avalibleUpgrades[randomUpgrade]);
                }
                else
                {
                    upgradebutton.upgradeString = "";
                    upgradebutton.gameObject.SetActive(false);
                }
            }
        }
    }

    void updateStats()
    {
        maxHealth = baseMaxHealth * (((healthLevel + PermenentUpgrades.knightHealth + PermenentUpgrades.knightLevel) * 0.1f) + 1);
        speed = baseSpeed * (((speedLevel + PermenentUpgrades.knightSpeed + PermenentUpgrades.knightLevel) * 0.1f) + 1);
        mainDamage = 5 + (2 * (PermenentUpgrades.knightDamage + PermenentUpgrades.knightLevel));
    }

    // Update is called once per frame
    void Update()
    {
        if (movement != Vector2.zero) animator.SetBool("Running", true);
        else animator.SetBool("Running", false);
        if (movement.normalized.y < 0) animator.SetBool("RunningBackward", true);
        else animator.SetBool("RunningBackward", false);


        /*Debug.Log(EXP);
        Debug.Log(EXPToNextLVL);
        Debug.Log(EXP / EXPToNextLVL);*/


        if (health > 0)
        {
            if (EXP >= EXPToNextLVL)
            {
                levelUP();
                EXP = 0;
            }

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            //Debug.Log(health);

            if(Time.timeScale > 0)
            {
                body.velocity = new Vector3(movement.x, 0, movement.y) * speed;
                //transform.position += new Vector3(movement.x * speed, 0, movement.y * speed) * Time.deltaTime;


                //Makes the player look at the cursor
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out float distance))
                {
                    playerModel.transform.LookAt(ray.GetPoint(distance));
                }
                //And rotates so it doesnt look upwards
                var rotationVector = playerModel.transform.rotation.eulerAngles;
                rotationVector.x = 0;

                playerModel.transform.rotation = Quaternion.Euler(rotationVector);
            }
            
        }
        else
        {
            animator.SetBool("Dead", true);
            body.isKinematic = true;
            deathUI.die();
        }
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnMainAttack(InputValue input)
    {
        if(health > 0)
        {
            if (PermenentUpgrades.autoAttack)
            {
                autoAttack = input.isPressed;
            }
            StartCoroutine(attack());

        }
    }

    bool doneAttacking()
    {
        return animator.GetBool("Attacking");
    }

    IEnumerator attack()
    {
        if(!hasAttacked)
        {
            hasAttacked = true;
            //Instantiate(swordAttack, attackSpawn.transform);
            soundController.playSound(1);
            animator.SetBool("Attacking", true);
            yield return new WaitUntil(doneAttacking);
            hasAttacked = false;
            if (autoAttack)
            {
                StartCoroutine(attack());
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            damage(20 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(health > 0)
        {
            if (other.CompareTag("Coin"))
            {
                Resources.Tempcoins += Random.Range(1, 11);
                Destroy(other.gameObject);
                SoundPlayer.playSound(6);
            }
            else if (other.CompareTag("Wood"))
            {
                Resources.Tempwood += Random.Range(1, 6) * (((int)Timer.time / 60) + 1);
                Destroy(other.gameObject);
                SoundPlayer.playSound(6);
            }
            else if (other.CompareTag("Stone"))
            {
                Resources.Tempstone += Random.Range(1, 4) * (((int)Timer.time / 60) + 1);
                Destroy(other.gameObject);
                SoundPlayer.playSound(6);
            }
            else if (other.CompareTag("Health"))
            {
                health += maxHealth * 0.3f;
                Destroy(other.gameObject);
            }
        }
    }

    public void damage(float damage)
    {
        if(health > 0)
        {
            health -= damage;
            StartCoroutine(playHitSound());
        }
    }

    IEnumerator playHitSound()
    {
        if (!hurt)
        {
            hurt = true;
            soundController.playSound(0);
            yield return new WaitForSeconds(0.25f);
            hurt = false;
        }
    }
}
