using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    private AmmoCount ammoCount;
    private AudioSource audioSource;

    [SerializeField] private AudioClip gunSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject pistol;
    //[SerializeField] private GameObject ammoCount;

    [SerializeField] private Animator anim;

    //Pistol Base Stats
    public int maxPistolAmmo = 60;
    public int reservePistolAmmo = 60;
    public int maxPistolMag = 12;
    public int currentPistolMag = 12;
    public float pistolReloadTime = 1;
    public float pistolDamage = 20;
    public float pistolFirerate = 0.25f;
    public float PistolSpread = 0.3f;
    public bool allowRicochet = false;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Image pistolIcon;

    //Gun Unlocks
    public bool pistolEquiped = true;

    bool isReloading;
    bool isFireing;

    bool colliding;

    [SerializeField] public bool infiniteAmmoCheat;

    private void Start()
    {
        ammoCount = FindObjectOfType<AmmoCount>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (infiniteAmmoCheat) reservePistolAmmo = maxPistolAmmo;
            if (pistolEquiped)
            {
                if (isReloading)
                    return;

                if (Input.GetMouseButtonDown(0) && PlayerManager.canGunsFire)
                {
                    StartCoroutine(fireGun());
                }
                if (Input.GetKeyDown("r") == true && currentPistolMag != maxPistolMag)
                {
                    StartCoroutine(reload());
                }
            }
        }

        if (pistolEquiped == true)
        {
            //ammoCount.SetActive(true);
            ammoCount.currentAmmoCount = currentPistolMag;
            ammoCount.totalMagSize = reservePistolAmmo;
        }
    }

    public void EquipPistol()
    {
        pistol.SetActive(true);
        pistolEquiped = true;

        FindObjectOfType<AmmoCount>().currentGunImage = pistolIcon;
    }

    public void DeEquipPistol()
    {
        pistol.SetActive(false);
        pistolEquiped = false;
    }

    IEnumerator fireGun()
    {
        if (!colliding)
        {
            if (!isFireing)
            {
                // Spawn bullet

                isFireing = true;
                var rotationVector = Player.transform.rotation.eulerAngles;
                rotationVector.x = 90;
                float rotationBase = rotationVector.y;

                if (currentPistolMag > 0)
                {
                    currentPistolMag--;
                    Debug.Log(currentPistolMag);

                    // Spawn bullet
                    float randomPitch = Random.Range(0.95f, 1.05f);

                    CameraEffects weaponKickback = FindObjectOfType<CameraEffects>();
                    StartCoroutine(weaponKickback.CameraZoom(60, 61, 0.20f));

                    audioSource.pitch = randomPitch;
                    audioSource.PlayOneShot(gunSound);
                    yield return new WaitForSeconds(Random.Range(0, 0.001f));
                    rotationVector.y = rotationBase + Random.Range(-PistolSpread, PistolSpread);
                    var rotation = Quaternion.Euler(rotationVector); //rotation stuffs
                    Instantiate(Bullet, bulletSpawnPoint.position, rotation);

                }
                else StartCoroutine(reload()); //If player out of ammo, auto reload.

                yield return new WaitForSeconds(pistolFirerate);
                isFireing = false;
            }
        }
    }

    IEnumerator reload()
    {
        anim.SetBool("Reloading", true);

        audioSource.PlayOneShot(reloadSound);

        isReloading = true;
        Debug.Log("Reloading");
        int bulletChange;

        yield return new WaitForSeconds(pistolReloadTime);
        anim.SetBool("Reloading", false);

        //check if you even have bullets to reload/need to reload
        if (reservePistolAmmo <= 0 || maxPistolMag == currentPistolMag)
        {
            anim.SetBool("Reloading", false);
            isReloading = false;
            yield break;
            
        }

        //Updates Current and Reserve Bullets
        bulletChange = maxPistolMag - currentPistolMag;
        if (bulletChange > reservePistolAmmo)
        {
            bulletChange = reservePistolAmmo;
        }
        currentPistolMag += bulletChange;
        reservePistolAmmo -= bulletChange;

        isReloading = false;
        anim.SetBool("Reloading", false);

        yield return null;
    }

    public void AddAmmo(int amount)
    {
        reservePistolAmmo += amount;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            colliding = false;
        }
    }
}
