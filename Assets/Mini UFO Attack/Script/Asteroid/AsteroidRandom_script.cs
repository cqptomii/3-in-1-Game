using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AsteroidRandom_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    protected Camera main_camera;
    protected Vector2 screen_bounds;
    protected Vector2 newPos;
    protected float object_rotation; // degree of rotation of the object
    protected float speed; // speed of the asteroid
    /*
    *
    *
    *
    */
    
    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        screen_bounds = main_camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,main_camera.transform.position.z));
        speed = Random.Range(5,15);
        object_rotation = Random.Range(10,40);
    }

    // Update is called once per frame
    void Update()
    {
        // Update position
        newPos = transform.position;
        newPos.x -= speed * Time.deltaTime;

        //Update rotation
        
        // Destroy object when it leave the camera FOV
        if(transform.position.x < -screen_bounds.x){
            Destroy(gameObject);
        }

        transform.position = newPos;
        transform.RotateAround(transform.position,Vector3.forward,object_rotation*Time.deltaTime);
    }
}
