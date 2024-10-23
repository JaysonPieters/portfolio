using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{
    private AmmoCount ammoCount;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject shotgun;
    //[SerializeField] private GameObject ammoCount;

    //[SerializeField] private Animator anim;

    //Shotgun Base Stats
    public int maxShotgunAmmo = 24;
    public int reserveShotgunAmmo = 24;
    public int maxShotgunMag = 6;
    public int currentShotgunMag = 6;
    public float shotgunReloadTime = 1;
    public float shotgunDamage = 10;
    public int shotgunBullets = 7;
    public float shotgunSpread = 10;
    public float shotgunFirerate = 1.5f;

    bool colliding;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Image shotgunIcon;

    //Gun Unlocks
    public bool shotgunEquiped = true;

    bool isFireing;
    bool isReloading;

    [SerializeField] public bool infiniteAmmoCheat;

    private void Start()
    {
        ammoCount = FindObjectOfType<AmmoCount>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (infiniteAmmoCheat) reserveShotgunAmmo = maxShotgunAmmo;
            if (shotgunEquiped)
            {
                if (isReloading)
                    return;

                if (Input.GetMouseButtonDown(0) && PlayerManager.canGunsFire)
                {
                    StartCoroutine(fireGun());
                }
                if (Input.GetKeyDown("r") == true)
                {
                    StartCoroutine(reload());
                }
            }
        }

        if (shotgunEquiped == true)
        {
            ammoCount.currentAmmoCount = currentShotgunMag;
            ammoCount.totalMagSize = reserveShotgunAmmo;
        }
    }

    public void EquipShotgun()
    {
        shotgun.SetActive(true);
        shotgunEquiped = true;

        FindObjectOfType<AmmoCount>().currentGunImage = shotgunIcon;
    }

    public void DeEquipShotgun()
    {
        shotgun.SetActive(false);
        shotgunEquiped = false;
    }

    IEnumerator fireGun()
    {
        if (!colliding)
        {
            if (!isFireing)
            {
                isFireing = true;
                int i = 0;
                var rotationVector = Player.transform.rotation.eulerAngles;
                rotationVector.x = 90;
                float rotationBase = rotationVector.y;

                if (currentShotgunMag > 0)
                {
                    currentShotgunMag--;
                    Debug.Log(currentShotgunMag);
                    // Spawn bullet

                    while (i < shotgunBullets)
                    {
                        rotationVector.y = rotationBase + Random.Range(-shotgunSpread, shotgunSpread);
                        var rotation = Quaternion.Euler(rotationVector); //rotation stuffs
                        Instantiate(Bullet, bulletSpawnPoint.position, rotation);
                        i++;

                        SoundManager.PlaySound(SoundManager.Sounds.shotgunShot, 0.01f);

                        yield return new WaitForSeconds(Random.Range(0, 0.0001f));

                        CameraEffects weaponKickback = FindObjectOfType<CameraEffects>();
                        StartCoroutine(weaponKickback.CameraZoom(60, 64, 0.6f));
                    }

                }
                else StartCoroutine(reload()); //If player out of ammo, auto reload.

                yield return new WaitForSeconds(shotgunFirerate);
                isFireing = false;
            }
        }

    }

    IEnumerator reload()
    {
        //anim.SetBool("Reloading", true);

        isReloading = true;
        int bulletChange;

        SoundManager.PlaySound(SoundManager.Sounds.shotgunReload, 0.3f);

        yield return new WaitForSeconds(shotgunReloadTime);
        //anim.SetBool("Reloading", false);

        //check if you even have bullets to reload/need to reload
        if (reserveShotgunAmmo <= 0 || maxShotgunMag == currentShotgunMag)
        {
            //anim.SetBool("Reloading", false);
            isReloading = false;
            yield break;
            
        }

        //Updates Current and Reserve Bullets
        bulletChange = maxShotgunMag - currentShotgunMag;
        if (bulletChange > reserveShotgunAmmo)
        {
            bulletChange = reserveShotgunAmmo;
        }
        currentShotgunMag += bulletChange;
        reserveShotgunAmmo -= bulletChange;

        isReloading = false;
        //anim.SetBool("Reloading", false);

        yield return null;
    }

    public void AddAmmo(int amount)
    {
        reserveShotgunAmmo += amount;
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
