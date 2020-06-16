using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("Vitesse de translation en m.s^-1")]
    [SerializeField] float m_translationSpeed;

    Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //recuperation de l'imput lier a l'axes horizontal
        float vInput = Input.GetAxis("Horizontal");

        Vector3 translationVect = vInput * transform.right * m_translationSpeed * Time.fixedDeltaTime;
        //referentiel world








        m_rigidbody.MovePosition(transform.position + translationVect);
    }

}
