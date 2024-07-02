using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BlueBall_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTES
    *   
    */ 
    private Camera current_camera;
    private Vector3 Screen_Bounds;
    private Rigidbody2D ref_current_rigidBody;
    private Vector2 Vector_init;

    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_current_rigidBody = GetComponent<Rigidbody2D>();
        current_camera = Camera.main;
        Screen_Bounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,current_camera.transform.position.z));
        
        Vector_init = new Vector2(Random.Range(-4,-7),Random.Range(-4f,4f));
        ref_current_rigidBody.velocity = Vector_init;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= -Screen_Bounds.x-1){
            Destroy(gameObject);
        }
    }
    /// <summary>
    ///     Trigger Collision handling with the player
    /// </summary>
    /// <param name="other">Collision informations of the object</param>
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "joueur"){
            Destroy(gameObject);
        }
    }
}
