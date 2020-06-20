using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    int random;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(90 * Time.deltaTime,0  ,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerScript script = other.GetComponentInParent<PlayerScript>();
        //Debug.Log("Collision");

        if (script.name == "Player")
        {
            random = Random.Range(1,3);
            Debug.Log("Random = "+random);
            if(random == 1)
            {
                Debug.Log("Inversion des axes");
                if (script.InvertAxes)
                {
                    script.InvertAxes = false;
                }
                else
                {
                    script.InvertAxes = true;
                }
            }
            if(random == 2)
            {
                Debug.Log("Vitesse augmenté");
                script.moveSpeed = 50;
                script.timeLeftSpeed = 5f;
            }
            if (random == 3)
            {
                Debug.Log("Vitesse diminué");
                script.moveSpeed = 2;
                script.timeLeftSpeed = 5f;
            }
            if (random == 4)
            {
                Debug.Log("Vitesse Jump diminué");
                script.m_JumpSpeed = 2;
                script.timeLeftJump = 5f;
            }
            if (random == 5)
            {
                Debug.Log("Vitesse Jump Augmenté");
                script.m_JumpSpeed = 20;
                script.timeLeftJump = 5f;
            }
            if(random == 6)
            {
                Debug.Log("Changement de caméra");
                script.SetPreset(2);
            }


            //other.GetComponent<PlayerScript>().InvertAxes=true;
            //other.GetComponent<PlayerScript>().points++;
            // Add 1 to points

            Destroy(gameObject);
        }
    }
}
