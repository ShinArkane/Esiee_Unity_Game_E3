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
        canJump = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool isGrounded()
    {
        return canJump;
    }

    private void FixedUpdate()
    {
        
        if (!GameManager.Instance.IsPlaying) return;
        Debug.Log("play");

        //recuperation de l'imput lier a l'axes horizontal
        float hInput = Input.GetAxis("Horizontal");

        Vector3 translationVect = hInput * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;

        if (Input.GetKey("up") && isGrounded())
        {
            m_Rigidbody.AddForce(new Vector3(0, m_JumpPower, 0), ForceMode.Impulse);
        }

        m_Rigidbody.MovePosition(transform.position + translationVect);



        // rester debout
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(qNextOrientation);
        

        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>()) canJump = true;

        // TODO A CHANGER 
        CubeBehaviour cube = collision.collider.GetComponent<CubeBehaviour>();
        if (cube)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                Debug.Log(point.normal);
                if (point.normal.y <= -0.9f)
                {
                    cube.Destruction(gameObject);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>()) canJump = false;
    }
}
