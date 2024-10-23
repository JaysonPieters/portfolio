using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBase))]

public class BasicEnemy : MonoBehaviour
{
    EnemyBase Base;
    Rigidbody body;
    [SerializeField] private AnimationCurve curve;

    bool hitPlayer;
    bool canHitPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Base = GetComponent<EnemyBase>();
        Base.health = 10;
        body = GetComponent<Rigidbody>();
        StartCoroutine(ExecuteBehaviourAsync());
        Base.speed = 3;
    }

    private IEnumerator ExecuteBehaviourAsync()
    {
        Transform player = FindObjectOfType<KnightBehavior>().transform;
        
        while (true)
        {
            float timer = 0f;
            while (Vector3.Distance(transform.position, player.position) > 4f)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                //body.AddForce(transform.forward * 1000 * Time.deltaTime);
                
                body.velocity = transform.forward * Base.speed;
                yield return null;
            }
            while (timer < .5f)
            {
                body.velocity = Vector3.zero;
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0f;
            while (timer < 1.5f)
            {
                if (!hitPlayer)
                {
                    body.velocity = transform.forward * Base.speed * 2.5f;
                    timer += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    body.velocity = -transform.forward * Base.speed * 1.5f;
                }
            }
            canHitPlayer = false;
            hitPlayer = false;
            /*Vector3 targetPosition = player.transform.position;
            Vector3 startingPosition = transform.position;
            float originalDistance = Vector3.Distance(startingPosition, targetPosition);
            body.velocity = Vector3.zero;
            while (true)
            {
                Vector3 position = Vector3.MoveTowards(startingPosition, targetPosition, 3f * Time.deltaTime);
                position.y = curve.Evaluate(Vector3.Distance(startingPosition, new Vector3(transform.position.x, startingPosition.y, transform.position.z))) * 8f;
                transform.position = position;
                yield return null;
                
            }*/
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canHitPlayer)
        {
            if (collision.gameObject.GetComponent<KnightBehavior>() != null)
            {
                hitPlayer = true;
            }
        }
    }
}
