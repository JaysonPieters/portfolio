using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigun : MonoBehaviour
{
    private AmmoCount ammoCount;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject miniGun;
    [SerializeField] PlayerBehaviour playerBehaviour;
    //[SerializeField] private GameObject ammoCount;

    //[SerializeField] private Animator anim;

    //AR Base Stats
    public int maxMinigunAmmo = 900;
    public int reserveMinigunAmmo = 900;
    public int maxMinigunMag = 300;
    public int currentMinigunMag = 300;
    public int minigunReloadTime = 5;
    public int minigunDamage = 5;
    public float minigunSpread = 20;
    public float minigunFirerate = 20;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Image minigunImage;

    //Gun Unlocks
    public bool minigunEquiped = true;

    //bool stateChanged = false;
    bool isFireing;
    bool isReloading;

    bool colliding;

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
            if (infiniteAmmoCheat) reserveMinigunAmmo = maxMinigunAmmo;
            if (minigunEquiped)
            {
                playerBehaviour.speed = playerBehaviour.speed = 300;
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
            else playerBehaviour.speed = playerBehaviour.speed = 500;
        }

        if (minigunEquiped == true)
        {
            ammoCount.currentAmmoCount = currentMinigunMag;
            ammoCount.totalMagSize = reserveMinigunAmmo;
        }
    }

    public void EquipMinigun()
    {
        miniGun.SetActive(true);
        minigunEquiped = true;
        FindObjectOfType<AmmoCount>().currentGunImage = minigunImage;
    }

    public void DeEquipMinigun()
    {
        miniGun.SetActive(false);
        minigunEquiped = false;
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

                if (currentMinigunMag > 0)
                {
                    currentMinigunMag--;
                    CameraEffects weaponKickback = FindObjectOfType<CameraEffects>();
                    StartCoroutine(weaponKickback.CameraZoom(60, 60.6f, 0.08f));

                    // Spawn bullet
                    yield return new WaitForSeconds(Random.Range(0, 0.001f));

                    SoundManager.PlaySound(SoundManager.Sounds.shotgunShot, 0.04f);

                    rotationVector.y = rotationBase + Random.Range(-minigunSpread, minigunSpread);
                    var rotation = Quaternion.Euler(rotationVector); //rotation stuffs
                    Instantiate(Bullet, bulletSpawnPoint.position, rotation);

                }
                else StartCoroutine(reload()); //If player out of ammo, auto reload.

                yield return new WaitForSeconds(minigunFirerate);
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

        yield return new WaitForSeconds(minigunReloadTime);
        //anim.SetBool("Reloading", false);

        //check if you even have bullets to reload/need to reload
        if (reserveMinigunAmmo <= 0 || maxMinigunMag == currentMinigunMag)
        {
            //anim.SetBool("Reloading", false);
            isReloading = false;
            yield break;
            
        }

        //Updates Current and Reserve Bullets
        bulletChange = maxMinigunMag - currentMinigunMag;
        if (bulletChange > reserveMinigunAmmo)
        {
            bulletChange = reserveMinigunAmmo;
        }
        currentMinigunMag += bulletChange;
        reserveMinigunAmmo -= bulletChange;

        isReloading = false;
        //anim.SetBool("Reloading", false);

        yield return null;
    }

    public void AddAmmo(int amount)
    {
        reserveMinigunAmmo += amount;
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
