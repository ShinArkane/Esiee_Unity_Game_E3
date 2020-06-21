using SDD.Events;
using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("Vitesse de translation en m.s^-1")]
    [SerializeField] private float m_TranslationSpeed;
    [SerializeField] private float m_DiggingSpeed; //vitesse passage tuyaux
    [SerializeField] private float m_UpRightRotKLerp; // maintenir debut
    [SerializeField] private float m_JumpPower; // force de saut

    [Header("Read Only Player State")]
    [SerializeField] private bool canJump = false;
    [SerializeField] private bool canDig = false; //peu traverser le sol/tuyaux
    [SerializeField] private bool isDiggingDownward = false; //etat bloquant de chute
    
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float invincibilityCounter;


    Rigidbody m_Rigidbody;
    

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        canJump = false;
        canDig = false;
        isDiggingDownward = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
        if (!GameManager.Instance.IsPlaying) return;
        Vector3 translationVect;
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if (invincibilityCounter > 0) 
        {
            invincibilityCounter -= Time.fixedDeltaTime;
        }

        if (isDiggingDownward)
        {
            translationVect = -transform.up * m_DiggingSpeed * Time.fixedDeltaTime;

        }
        else { 
            //recuperation de l'imput lier a l'axes horizontal
            

            translationVect = hInput * transform.forward * m_TranslationSpeed * Time.fixedDeltaTime;

            if (Input.GetAxis("Jump") != 0 && canJump)
            {
                m_Rigidbody.AddForce(new Vector3(0, m_JumpPower, 0), ForceMode.Impulse);
            }

        }

        m_Rigidbody.MovePosition(transform.position + translationVect);

        // rester debout
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(qNextOrientation);


        if (vInput < 0 && canDig)
        {
            isDiggingDownward = true;
            canJump = false;
            canDig = false;
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Tuyaux>() && !isDiggingDownward) canDig = true;
        if (collision.gameObject.GetComponent<Tuyaux>() && isDiggingDownward) collision.collider.isTrigger = true;
        if (collision.gameObject.GetComponent<Ground>()) canJump = true;
        
    }



    private void OnCollisionEnter(Collision collision)
    {
        Ennemy_partrouille enemy = collision.collider.GetComponent<Ennemy_partrouille>();
        CubeBehaviour cube = collision.collider.GetComponent<CubeBehaviour>();
        Tuyaux tuyaux = collision.collider.GetComponent<Tuyaux>();
        Ground ground = collision.collider.GetComponent<Ground>();
        if ((ground || tuyaux) && isDiggingDownward) collision.collider.isTrigger = true;
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
        if (enemy)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                if (point.normal.y >= 0.6f)
                {
                    enemy.ApplyDommage();
                }
                else
                {
                    ApplyDommage();
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>()) canJump = false;
        if (collision.gameObject.GetComponent<Tuyaux>() && !isDiggingDownward) canDig = false;
        if (collision.gameObject.GetComponent<Ennemy_partrouille>()) canJump = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Tuyaux>()) {
            other.isTrigger = false;
            isDiggingDownward = false;
        }
        if (other.gameObject.GetComponent<Ground>() && isDiggingDownward)
        {
            other.isTrigger = false;
        }
        
    }

    public void ApplyDommage()
    {
        if(invincibilityCounter <= 0)
        {
            EventManager.Instance.Raise(new LifeEvent() { eLife = 1 });
            if (GameManager.Instance.NLives <= 0)
            {
                Destroy(gameObject);
                EventManager.Instance.Raise(new GameOverEvent());
            }
            else
            {
                invincibilityCounter = invincibilityTime;
            }
        }
        
        
    }

}
