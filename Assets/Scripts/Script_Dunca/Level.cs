using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int ID { get; set; }
    public string LevelName { get; set; }
    public bool Completed { get; set; }
    public int Stars { get; set; }
    public bool Locked { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }
    public Level(int id,string levelname,bool completed,int stars,bool locked)
    {
        this.ID = id;
        this.LevelName = levelname;
        this.Completed = completed;
        this.Stars = stars;
        this.Locked = locked;
    }
    public void Complete()
    {
        this.Completed = true;
    }
    public void Complete(int stars)
    {
        this.Completed = true;
        this.Stars = stars;
    }
    public void Lock()
    {
        this.Locked = true;
    }
    public void Unlock()
    {
        this.Locked = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
