using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_partrouille : MonoBehaviour
{

    [Tooltip("Vitesse de translation en m.s-1")]
    [SerializeField] private float m_TranslationSpeed;
    [SerializeField] private float m_PlayerDetectedSpeed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_UpRightRotKLerp;
    [SerializeField] private float m_detectionScalaire;

    [Header("Read Only")]
    [SerializeField] private bool usePlayerDetected;

    Rigidbody m_Rigidbody;

    static Vector3 translationVect;
    // Start is called before the first frame update

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward*10, Color.red);
        if (!GameManager.Instance.IsPlaying) return;

        SelectSpeed();

        m_Rigidbody.MovePosition(transform.position + translationVect);
        //float deltaAngle =  m_RotationSpeed * Time.deltaTime;
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(qNextOrientation);
    }

    public void OnCollisionEnter(Collision collision)
    {
         //TODO collision joueur  
    }

    private void SelectSpeed()
    {
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward * m_detectionScalaire);
        int layerMask = LayerMask.GetMask("Default");
        if (Physics.Raycast(ray, out hit,m_detectionScalaire,layerMask,QueryTriggerInteraction.Ignore))
        {
            Debug.Log("gameogbject =" + hit.transform.name);
            Debug.Log("gameogbject =" + (hit.transform.GetComponent<Player>() != null) );

            if ( hit.transform.gameObject.GetComponent<Player>() )
            {
                usePlayerDetected = true;
                translationVect = transform.forward * m_PlayerDetectedSpeed * Time.fixedDeltaTime;
            }
            else
            {
                translationVect = transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;
                usePlayerDetected = false;
            }

        }
        else
        {
            translationVect = transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TriggerMoveEnnemy>() )
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y *-1, transform.rotation.z,0.707f);
            translationVect *= -1;

        }
    }

}
