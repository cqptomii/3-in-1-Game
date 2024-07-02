using UnityEngine;

public class Apple_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10 )
            Destroy(gameObject);
    }
    /// <summary>
    ///     method to manage Collision between apple projectile and Player
    /// </summary>
    /// <param name="other"> Collisions details of the object </param>
    public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "panier"){
            Destroy(gameObject);
        }
    }
}
