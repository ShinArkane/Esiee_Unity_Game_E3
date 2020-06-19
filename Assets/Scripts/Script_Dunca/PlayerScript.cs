using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour
{

    #region Variables
    // Main variables for physics handling
    private Rigidbody rb;

    public float moveSpeed;
    bool canJump = false;
    [SerializeField] float m_JumpSpeed;

    dg_simpleCamFollow1 followScript;


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

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * moveSpeed);
        if ((Input.GetKeyDown(KeyCode.Space))&& isGrounded())
        {
            Debug.Log("Jump");
            rb.AddForce(new Vector3(0, m_JumpSpeed, 0), ForceMode.Impulse);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject && !collision.gameObject.name.Contains("Enemy"))
        {
            Debug.Log(collision.gameObject);
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
                Debug.Log(point.normal);
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
        Debug.Log(canJump);
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