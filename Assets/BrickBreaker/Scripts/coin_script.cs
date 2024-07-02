using UnityEngine;

public class coin_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if(gameObject.transform.position.y <-5){
            Destroy(gameObject);
       } 
    }

    /// <summary>
    ///     Method to destroy coins gameobject when it collide with our pad
    ///     Collisions with box / ball are disable with Collisions layer
    /// </summary>
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "pad"){
            Destroy(gameObject);
        }
    }
}
