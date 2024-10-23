using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float Damage;
    public float duration;
    public float AttackRange;
    public float slowAmount;

    bool destroying;

    bool hasAttacked;

    //[SerializeField] GameObject player;

    List<EnemyBase> enemiesInRange = new List<EnemyBase>();


    private void Start()
    {
        updateStats();
        StartCoroutine(destroy());
    }

    void updateStats()
    {
        Damage = Abilities.poisonDamage;
        duration = Abilities.poisonDuration;
        AttackRange = Abilities.poisonRange;
        slowAmount = Abilities.poisonSlow;
        
        transform.localScale = new Vector3(AttackRange,AttackRange,AttackRange);
        //transform.localScale = new Vector3(AttackRange / 10, 0.2f, AttackRange / 10);
    }

    // Start is called before the first frame update
    private void Update()
    {
        //updateStats();
        //transform.position = player.transform.position;
        StartCoroutine(damage());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!destroying)
        {
            if (other.GetComponent<EnemyBase>())
            {
                enemiesInRange.Add(other.GetComponent<EnemyBase>());
                //Debug.Log("Added " + other.name + " to the list");
                other.GetComponent<EnemyBase>().speed /= slowAmount;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyBase>())
        {
            enemiesInRange.Remove(other.GetComponent<EnemyBase>());
            //Debug.Log("Removed " + other.name + " from the list");
            other.GetComponent<EnemyBase>().speed *= slowAmount;
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(duration);
        destroying = true;
        yield return new WaitForEndOfFrame();
        foreach(EnemyBase enemy in enemiesInRange)
        {
            try
            {
                enemy.GetComponent<EnemyBase>().speed *= slowAmount;
            }
            catch (System.Exception)
            {

            }
        }
        Destroy(gameObject);
    }

    IEnumerator damage()
    {
        if (!hasAttacked)
        {
            hasAttacked = true;
            foreach (var enemy in enemiesInRange)
            {
                try
                {
                    enemy.damage(Damage);
                }
                catch (System.Exception)
                {
                }
                
            }
            yield return new WaitForSeconds(1);
            hasAttacked = false;
        }
    }
}
