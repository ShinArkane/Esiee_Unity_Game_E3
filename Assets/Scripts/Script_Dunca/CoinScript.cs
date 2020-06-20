﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    bool isUse = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,90 * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerScript script = other.GetComponentInParent<PlayerScript>();
        Debug.Log("Collision");

        if (script.name == "Player")
        {
            //other.GetComponent<PlayerScript>().points++;
            // Add 1 to points
            if (!isUse)
            {
                script.points++;
                isUse = true;
            }
            Destroy(gameObject);
        }
    }
}