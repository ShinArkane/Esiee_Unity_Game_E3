using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour
{

    #region Variables
    // Main variables for physics handling
    private Rigidbody rb;
    public bool InvertAxes = false;
    public float moveSpeed;
    bool canJump = false;
    [SerializeField] public float m_JumpSpeed;
    float timeLeftInvert = 5f;
    dg_simpleCamFollow1 followScript;
    public float timeLeftSpeed = -1;
    public float timeLeftJump = -1;
    float initiateMoveSpeed;
    float initiate_m_JumpSpeed;
    // Misc.
    private LineRenderer lr;

    // Points : 
    public int points = 0;
    #endregion
    private void Start()
    {
        Application.targetFrameRate = 60;
        // Lock rotation of rb 
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
        initiateMoveSpeed = moveSpeed;
        initiate_m_JumpSpeed = m_JumpSpeed;
        //Camera 
        followScript = Camera.main.GetComponent<dg_simpleCamFollow1>();
        SetPreset(1);

    }
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();

    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement;
        timeLeftInvert -= Time.deltaTime;
        timeLeftSpeed -= Time.deltaTime;
        timeLeftJump -= Time.deltaTime;
        if (timeLeftInvert < 0)
        {
            InvertAxes = false;
            timeLeftInvert = 20f;
        }

        if (timeLeftSpeed<0)
        {
            moveSpeed = initiateMoveSpeed;
        }
        if (timeLeftJump < 0)
        {
            m_JumpSpeed = initiate_m_JumpSpeed;
        }
        if (!InvertAxes)
        {
             movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        }
        else
        {
             movement = new Vector3(moveVertical, 0.0f, moveHorizontal);

        }

        rb.AddForce(movement * moveSpeed);
        if ((Input.GetKeyDown(KeyCode.Space))&& isGrounded())
        {
            rb.AddForce(new Vector3(0, m_JumpSpeed, 0), ForceMode.Impulse);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject && !collision.gameObject.name.Contains("Enemy"))
        {
            //Debug.Log(collision.gameObject);
            canJump = true;

        }

        EnemyBehaviour enemy = collision.collider.GetComponent < EnemyBehaviour >();
        CubeBehaviour cube = collision.collider.GetComponent<CubeBehaviour>();
        if(enemy)
        {
            //Debug.Log(enemy);
            //Debug.Log("3d " + collision);
            foreach(ContactPoint point in collision.contacts)
            {
                if(point.normal.y>=0.6f)
                {
                    enemy.Destruction();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        if (cube)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                //Debug.Log(point.normal);
                if (point.normal.y <= -0.9f)
                {
                    cube.Destruction(gameObject);
                }
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) SetPreset(1);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) SetPreset(2);
        // Hold
        if (Input.GetMouseButton(0))
        { 
        }
        // Release
        else
        {
        }
        // Update rope positions
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (collision.gameObject && !collision.gameObject.name.Contains("Enemy"))
        {
            canJump = false;

        }


    }

    public bool isGrounded()
    {
        //Debug.Log(canJump);
        return canJump;
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Score :" + points);
    }
    public void SetPreset(int i)
    {
        switch (i)
        {
            case 1:
                followScript.generalOffset = new Vector3(0, 0.2f, -10);
                followScript.lookAtTarget = false;
                followScript.laziness = 10;
                Camera.main.orthographic = false;
                Camera.main.transform.rotation = Quaternion.identity;
                break;

            case 2:
                followScript.generalOffset = new Vector3(0, 0, -15);
                followScript.lookAtTarget = false;
                followScript.laziness = 20;
                Camera.main.orthographic = true;
                Camera.main.transform.rotation = Quaternion.identity;
                break;
        }

    }
    

}