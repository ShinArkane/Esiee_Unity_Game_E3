using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Cube :"+CCollider.bounds.center.y);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        /*PlayerScript script = other.GetComponentInParent<PlayerScript>();
        Debug.Log(script);
        if (script.name == "Player")
        {

            //other.GetComponent<PlayerScript>().points++;
            // Add 1 to points
            script.points++;
            Destroy(gameObject);
        }*/
    }
    public void Destruction()
    {
        Destroy(gameObject);
    }
}
