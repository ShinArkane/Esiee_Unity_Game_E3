using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("Vitesse de translation en m.s^-1")]
    [SerializeField] float m_TranslationSpeed;
    [SerializeField] float m_UpRightRotKLerp; // maintenir debut
    [SerializeField] float m_JumpPower; // force de saut

    Rigidbody m_Rigidbody;
    bool canJump = false;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public bool isGrounded()
    {
        return canJump;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        //recuperation de l'imput lier a l'axes horizontal
        float hInput = Input.GetAxis("Horizontal");

        Vector3 translationVect = hInput * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;

        if (Input.GetKey("Space") && isGrounded())
        {
            //translationVect.y = m_JumpSpeed * Time.fixedDeltaTime;
            m_Rigidbody.AddForce(new Vector3(0, m_JumpPower, 0), ForceMode.Impulse);
        }

        m_Rigidbody.MovePosition(transform.position + translationVect);



        // rester debout
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(qNextOrientation);
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject && !collision.gameObject.name.Contains("Enemy"))
        {
            canJump = false;

        }


    }
}
