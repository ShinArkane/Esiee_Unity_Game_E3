using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    public bool isBreakable=true;
    bool CubeState = false;
    public bool HasItem = false;
    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Destruction(GameObject Player)
    {
        if (!CubeState)
        {
            Player script = Player.GetComponent<Player>();
            //script.points++; // changer pour des events
            CubeState = true;
            if (isBreakable)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<Renderer>().material.color =  Color.grey; //C sharp
                if(HasItem)
                {
                    Debug.Log(gameObject.transform.position.x);
                    Instantiate(myPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1, gameObject.transform.position.z), Quaternion.identity);
                }
            }
        }
    }
}
