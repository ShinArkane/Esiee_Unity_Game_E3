using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_yannis : MonoBehaviour
{

    [Tooltip("Vitesse de translation en m.s-1")]

    [SerializeField] float m_TranslationSpeed;
    Rigidbody m_Rigidbody;
    [SerializeField] float m_UpRightRotKLerp;
    static float speed;

    static Vector3 translationVect;
    // Start is called before the first frame update

    private void Awake()
    {
        speed = m_TranslationSpeed;
        m_Rigidbody = GetComponent<Rigidbody>();
        translationVect = new Vector3(1, 0, 0) * speed * Time.deltaTime;
    }
    void Start()
    {
        speed = m_TranslationSpeed;

    }

    public  static Vector3 Speed
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
   

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Play");
        if (!GameManager.Instance.IsPlaying) return;
        //if (!GameManager.IsPlaying) return;
        //if (!GameManager.IsPlaying) return;
        transform.Translate(translationVect, Space.Self);
       
        m_Rigidbody.MovePosition(transform.position + translationVect);
        Quaternion qUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
        Quaternion qNextOrientation = Quaternion.Lerp(transform.rotation, qUpright * transform.rotation, m_UpRightRotKLerp * Time.fixedDeltaTime);
        m_Rigidbody.MoveRotation(qNextOrientation);
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.name.Contains("EnemieWall"))

        {
            //Debug.Log("collision");
            translationVect = -translationVect;

        }
    
    }
}
