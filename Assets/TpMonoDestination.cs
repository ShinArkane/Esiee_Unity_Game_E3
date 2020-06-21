using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpMonoDestination : MonoBehaviour
{
    [SerializeField] Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = destination;
    }
}
