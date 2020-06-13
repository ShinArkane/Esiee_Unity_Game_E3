using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("Vitesse de translation en m.s-1")]
    [SerializeField] float m_TranslationSpeed;
    [SerializeField] float m_UpRightRotKLerp;
    [SerializeField] float m_JumpSpeed;

    Rigidbody m_Rigidbody;
    bool canJump=false;

    // [SerializeField] float m_UpRightRotKLerp;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //m_ShotNextTime = Time.time;
        Debug.Log(Time.frameCount + " - Start");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject && !collision.gameObject.name.Contains("Enemie"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject && !collision.gameObject.name.Contains("Enemie"))
        {
            canJump = false;
        }

    }

    public bool isGrounded()
    {
        return canJump;
    }


    private void FixedUpdate()
    {



        float moveHorizontal = Input.GetAxis("Horizontal");
        Debug.Log("MoveHorizentale: " + moveHorizontal);
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 translationVect = moveHorizontal * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;
 

        if (Input.GetKey("up")&&isGrounded())
        {
            translationVect.y = m_JumpSpeed * Time.fixedDeltaTime;
        }
        m_Rigidbody.MovePosition(transform.position + translationVect);
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
      //  m_Rigidbody.MovePosition(transform.position + translationVect);
        m_Rigidbody.MoveRotation(qNextOrientation);
        // rb.AddForce(movement * m_TranslationSpeed);

        /*        if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
                }*/

    }
}
