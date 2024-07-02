using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallE3_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    private Camera current_camera; 
    private Vector3 Screen_Bounds;
    private float speed; // speed of the object
    private Vector2 NewPos;
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        current_camera = Camera.main;
        Screen_Bounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,current_camera.transform.position.z));
        speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Handle_position();
    }
    /// <summary>
    ///     Methode to update the position of the object in the scene
    /// </summary>
    void Handle_position(){
        if(gameObject.transform.position.x == -Screen_Bounds.x - 1){
            Destroy(gameObject);
        }
        NewPos = transform.position;
        NewPos.x -= speed * Time.deltaTime;
        transform.position = NewPos;
    }

    /// <summary>
    ///       Method to catch Trigger Collision with the player
    /// </summary>
    /// <param name="other"> Collisions information of the object</param>
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "joueur"){
            Destroy(gameObject);
        }
    }
}
