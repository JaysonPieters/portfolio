using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float maxLifetime = 3;
    float lifetime;

    [SerializeField] float speed = 3;
    private float startRotation;

    private Rigidbody bullet;
    private CapsuleCollider coll;

    Pistol pistol;
    public float damage;

    void Start()
    {
        coll = GetComponent<CapsuleCollider>();

        bullet = GetComponent<Rigidbody>();
        pistol = FindObjectOfType<Pistol>();
        var rotationVector = transform.rotation.eulerAngles;
        rotationVector.x = startRotation;
        transform.rotation = Quaternion.Euler(rotationVector);
        damage = pistol.pistolDamage;
        switch (WeaponWheelController.weaponID)
        {
            case 1:
                damage = 20;
                break;

            case 2:
                damage = 20;
                break;

            case 3:
                damage = 8;
                break;

            case 4:
                damage = 16;
                break;

            case 5:
                damage = 8;
                break;
        }
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime) Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        bullet.velocity = (transform.forward * speed * 2 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EndBoss>())
        {
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);

            other.gameObject.GetComponent<EndBoss>().DamageTaken(damage);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        TrainingDummy trainingDummy = other.transform.GetComponent<TrainingDummy>();
        EnemyOne enemyOne = other.transform.GetComponent<EnemyOne>();
        PlayerBehaviour player = other.transform.GetComponent<PlayerBehaviour>();
        MiniBoss miniBoss = other.transform.GetComponent<MiniBoss>();
        ExplosiveBarrel barrel = other.transform.GetComponent<ExplosiveBarrel>();

        if (other.gameObject.GetComponent<RapidBlast>())
        {
            other.gameObject.GetComponent<RapidBlast>().DamageTaken(damage);
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);
            Destroy(gameObject);
        }
        if (other.gameObject.GetComponent<BlazeBot>())
        {
            other.gameObject.GetComponent<BlazeBot>().DamageTaken(damage);
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);
            Destroy(gameObject);
        }

        if (trainingDummy != null)
        {
            trainingDummy.DamageTaken(damage);
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);
            Destroy(gameObject);
        }
        else if (enemyOne != null)
        {
            enemyOne.DamageTaken(damage);
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);
            coll.enabled = false;

            Destroy(gameObject);
        }
        else if (miniBoss != null)
        {
            miniBoss.DamageTaken(damage);
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);
            Destroy(gameObject);

        }
        else if (player != null)
        {
            return;
        }
        else if (barrel != null)
        {
            SoundManager.PlaySound(SoundManager.Sounds.hitSound, 0.09f);
            barrel.Explode();
            Destroy(gameObject);

        }

        if (pistol.allowRicochet)
        {
            Vector3 direction = ((transform.position + transform.forward) - transform.position).normalized;
            Vector3 reflect = Vector3.Reflect(direction, other.transform.forward);
            SoundManager.PlaySound(SoundManager.Sounds.bulletImpact, 0.02f);
            transform.rotation = Quaternion.LookRotation(reflect);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
