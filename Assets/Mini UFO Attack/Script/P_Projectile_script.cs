using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Projectile_script : MonoBehaviour
{

    /*
    *
    *   ATTRIBUTE
    *
    */
    protected Camera main_camera;
    protected Vector2 screenBounds;
    protected Rigidbody2D projectile_body; // Current rigidbody of the projectile
    protected Vector2 init_velocity = new Vector2(15f,0f); // initial velocity of a projectile
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        screenBounds = main_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, main_camera.transform.position.z));
        projectile_body = GetComponent<Rigidbody2D>();
        if(projectile_body){
            projectile_body.velocity = init_velocity;
        }else{
            Debug.LogError("No Rigidbody attached to the object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > screenBounds.x + 2){
            Destroy(gameObject);
        }
    }
}
