using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3[] points; // liste de position de destination
    private int point_number = 0; // nombre de position
    private Vector3 currentTarget; //vers ou ont ce dirige

    [SerializeField] private float speed; // vitesse de la plateforme

    [SerializeField] private bool automaticMove;

    private float tolerance; // permet a la plateforme de s'ajuster correctement a la destination
    [SerializeField] private float delay_time; // temps d'attente avant de bouger entre deux points

    private float delay_start;

    // Start is called before the first frame update
    void Start()
    {
        if (points.Length >0)
        {
            currentTarget = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != currentTarget)
        {
            MovePlateform();
        }
        else {
            UpdateTarget();
        }
    }


    private void MovePlateform()
    {
        Vector3 heading = currentTarget - transform.position;
        //magnitude => longeur entre (0,0,0) et notre point dans l'espace
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        // ajuster quand il reste une valeur insignifiante a ce deplacer
        if (heading.magnitude < tolerance)
        {
            transform.position = currentTarget;
            delay_start = Time.time; // commencer notre timer
        }
    }

    private void UpdateTarget()
    {
        if (automaticMove)
        {
            if (Time.time - delay_start > delay_time)
            {
                GoToNextPlatform();
            }
        }

    }

    //public pour declencher quand pas automatique
    public void GoToNextPlatform()
    {
        point_number++;
        if (point_number >= points.Length)
        {
            point_number = 0;
        }
        currentTarget = points[point_number];
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }

}
