using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    public Rigidbody target;

    public float maxSpeed = 0.0f;
    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    public RectTransform arrow;
    private float speed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        speed = target.velocity.magnitude * 3.6f;

        arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
    }
}
