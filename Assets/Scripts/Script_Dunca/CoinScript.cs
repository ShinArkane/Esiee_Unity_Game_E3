using SDD.Events;
using STUDENT_NAME;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] int scoreIncrementation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;
        transform.Rotate(new Vector3(0,20,0) * Time.deltaTime);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            EventManager.Instance.Raise(new ScoreItemEvent() { eScore = scoreIncrementation });
            Destroy(gameObject);
        }
    }
    
}
