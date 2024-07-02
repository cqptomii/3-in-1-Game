using System.Collections;
using UnityEngine;

public class Bonus_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */
    private float speed;
    private AudioSource ref_bonus_Audiosource;
    protected Camera main_camera;
    protected Vector2 screen_bounds;
    protected Vector3 newPos;
    protected bool is_destroy = false;
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_bonus_Audiosource = GetComponent<AudioSource>();
        ref_bonus_Audiosource.volume = 0.5f;
        ref_bonus_Audiosource.loop = false;

        main_camera = Camera.main;
        screen_bounds = main_camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,main_camera.transform.position.z));
        
        speed = Random.Range(5,10);
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
    }

    /// <summary>
    ///     Trigger Collisions handling
    /// </summary>
    /// <param name="other">Collisions informations of the object</param>
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "joueur"){
            Destroy(gameObject);
        }
    }

}
