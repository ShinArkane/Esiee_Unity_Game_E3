
using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{

    [Tooltip("Vitesse de translation en m.s-1")]

    [SerializeField] private float m_TranslationSpeed;
    [SerializeField] private float m_AccelerationSpeed;
    [SerializeField] private float m_RotationSpeed;
    Rigidbody m_Rigidbody;
    [SerializeField] private float m_UpRightRotKLerp;
    static float speed;
    static float acceleration;

    static Vector3 translationVect;
    // Start is called before the first frame update

    private void Awake()
    {
        speed = m_TranslationSpeed;
        acceleration = m_AccelerationSpeed;
        m_Rigidbody = GetComponent<Rigidbody>();
        translationVect = new Vector3(1, 0, 0) * speed * Time.deltaTime;
    }
    void Start()
    {
        speed = m_TranslationSpeed;

    }

    public static Vector3 Speed
    {
        get
        {
            return translationVect;
        }
        set
        {
            translationVect = value;
        }

    }

    public static float Acceleration
    {
        get
        {
            return acceleration;
        }
    }


    // Update is called once per frame
    void Update()
    {

        /* if (!GameManager.Instance.IsPlaying)
         {
             Debug.Log("Play");
             return;
         }*/
        transform.Translate(-translationVect, Space.Self);

        m_Rigidbody.MovePosition(transform.position + translationVect);
        //float deltaAngle =  m_RotationSpeed * Time.deltaTime;
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(qNextOrientation);
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name.Contains("EnemieWall"))

        {

            //translationVect = -translationVect;
            transform.Rotate(Vector3.up * 180);

        }

    }
}
