﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdMove : MonoBehaviour
{
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float rotateSpeed = 150.0f;

    private float forwardSpeed = 10.0f;

    private bool flying = true;

    private float maxPower = 10.0f;
    private float power;
    private float rechargeFactor = 3.0f;

    private Slider powerSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        powerSlider = GameObject.Find("Power Slider").GetComponent<Slider>();
        power = maxPower;
    }

    // Update is called once per frame
    void Update()
    {
        // get player input
        pitch += -Input.GetAxis("Vertical") * Time.deltaTime * rotateSpeed;
        yaw += Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed;

        pitch = Mathf.Clamp(pitch, -30, 90);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        
        if (flying)
        {
            if (power > 0)
            {
                transform.Translate(forwardSpeed * Time.deltaTime * Vector3.forward);
                ChangePower(-Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(0, -2.0f * Time.deltaTime, 0));
            }
        }
        else
        {
            ChangePower(rechargeFactor * Time.deltaTime);
            if (Input.GetKey(KeyCode.Space))
            {
                // transform.Translate()
                flying = true;
            }
        }
    }

    void ChangePower(float change)
    {
        power += change;
        if (power > maxPower) power = maxPower;
        powerSlider.value = power/maxPower;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Power Line")
        {
            // sit on power line
            flying = false;
            float height = other.gameObject.transform.position.y + 1;

            if (other.gameObject.GetComponent<PowerLine>().IsXCentered())
            {
                transform.position = new Vector3(other.gameObject.transform.position.x, height, transform.position.z);
            }
            else transform.position = new Vector3(transform.position.x, height, other.gameObject.transform.position.z);
            
            // Vector3 fromCenter = transform.position - other.gameObject.transform.position;

            // if (fromCenter.x < fromCenter.z)
            // {
            //     transform.position = new Vector3(transform.position.x, height, fromCenter.z);
            // }
            // else transform.position = new Vector3(fromCenter.x, height, transform.position.z);
        }
    }
}
