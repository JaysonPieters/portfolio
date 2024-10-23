using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AR : MonoBehaviour
{
    private AmmoCount ammoCount;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject assaultRifle;
    //[SerializeField] private GameObject ammoCount;

    //[SerializeField] private Animator anim;

    //AR Base Stats
    public int maxARAmmo = 180;
    public int reserveARAmmo = 180;
    public int maxARMag = 30;
    public int currentARMag = 30;
    public float ARReloadTime = 1;
    public float ARDamage = 5;
    public float ARSpread = 10;
    public float ARFirerate = 1.5f;

    [SerializeField] private Transform bulletSpawnPoint;

    //Gun Unlocks
    public bool AREquiped = true;

    bool isFireing;
    bool isReloading;

    bool colliding;

    [SerializeField] public bool infiniteAmmoCheat;
    [SerializeField] private Image assaultRifleIcon;

    private void Start()
    {
        ammoCount = FindObjectOfType<AmmoCount>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (infiniteAmmoCheat) reserveARAmmo = maxARAmmo;
            if (AREquiped)
            {
                if (isReloading)
                    return;

                if (Input.GetMouseButton(0) && PlayerManager.canGunsFire)
                {
                    StartCoroutine(fireGun());
                }
                if (Input.GetKeyDown("r") == true)
                {
                    StartCoroutine(reload());
                }
            }
        }

        if (AREquiped == true)
        {
            ammoCount.currentAmmoCount = currentARMag;
            ammoCount.totalMagSize = reserveARAmmo;
        }
    }

    public void EquipAssaultRifle()
    {
        assaultRifle.SetActive(true);
        AREquiped = true;
        FindObjectOfType<AmmoCount>().currentGunImage = assaultRifleIcon;
    }

    public void DeEquipRifle()
    {
        assaultRifle.SetActive(false);
        AREquiped = false;
    }

    IEnumerator fireGun()
    {
        if (!colliding)
        {
            if (!isFireing)
            {
                isFireing = true;
                var rotationVector = Player.transform.rotation.eulerAngles;
                rotationVector.x = 90;
                float rotationBase = rotationVector.y;

                if (currentARMag > 0)
                {
                    currentARMag--;
                    Debug.Log(currentARMag);

                    // Spawn bullet
                    yield return new WaitForSeconds(Random.Range(0, 0.001f));
                    rotationVector.y = rotationBase + Random.Range(-ARSpread, ARSpread);
                    var rotation = Quaternion.Euler(rotationVector); //rotation stuffs
                    Instantiate(Bullet, bulletSpawnPoint.position, rotation);

                    SoundManager.PlaySound(SoundManager.Sounds.assaultRifle, 0.08f);

                    CameraEffects weaponKickback = FindObjectOfType<CameraEffects>();
                    StartCoroutine(weaponKickback.CameraZoom(60, 60.65f, 0.12f));

                }
                else StartCoroutine(reload()); //If player out of ammo, auto reload.

                yield return new WaitForSeconds(ARFirerate);
                isFireing = false;
            }
        }

    }

    IEnumerator reload()
    {
        //anim.SetBool("Reloading", true);

        isReloading = true;
        Debug.Log("Reloading");
        int bulletChange;

        yield return new WaitForSeconds(ARReloadTime);
        //anim.SetBool("Reloading", false);

        //check if you even have bullets to reload/need to reload
        if (reserveARAmmo <= 0 || maxARMag == currentARMag)
        {
            //anim.SetBool("Reloading", false);
            isReloading = false;
            yield break;
            
        }

        //Updates Current and Reserve Bullets
        bulletChange = maxARMag - currentARMag;
        if (bulletChange > reserveARAmmo)
        {
            bulletChange = reserveARAmmo;
        }
        currentARMag += bulletChange;
        reserveARAmmo -= bulletChange;

        isReloading = false;
        //anim.SetBool("Reloading", false);

        yield return null;
    }

    public void AddAmmo(int amount)
    {
        reserveARAmmo += amount;
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
