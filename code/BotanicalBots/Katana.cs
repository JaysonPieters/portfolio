using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Katana : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public bool katanaEquiped = false;

    [SerializeField] private float range = 0.5f;
    [SerializeField] private int damage = 30;
    [SerializeField] private float attackRate;

    private float nextAttackTime;

    [SerializeField] private GameObject katana;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private Image katanaImage;

    private void Update()
    {
        if (katanaEquiped == true)
        {
            if (Time.time >= nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        SoundManager.PlaySound(SoundManager.Sounds.swordSlash, 0.2f);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, range, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.GetComponent<MiniBoss>() != null)
            {
                enemy.GetComponent<MiniBoss>().DamageTaken(damage);
            }
            if (enemy.GetComponent<TrainingDummy>() != null)
            {
                enemy.GetComponent<TrainingDummy>().DamageTaken(damage);
            }
            if (enemy.GetComponent<RapidBlast>() != null)
            {
                enemy.GetComponent<RapidBlast>().DamageTaken(damage);
            }
            if (enemy.GetComponent<BlazeBot>() != null)
            {
                enemy.GetComponent<BlazeBot>().DamageTaken(damage);
            }
            if (enemy.GetComponent<EndBoss>() != null)
            {
                enemy.GetComponent<EndBoss>().DamageTaken(damage);
            }
        }
    }

    public void EquipKatana()
    {
        katana.SetActive(true);
        katanaEquiped = true;
        FindObjectOfType<AmmoCount>().currentGunImage = katanaImage;
    }

    public void DeEquipKatana()
    {
        katana.SetActive(false);
        katanaEquiped = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, range);
    }
}
