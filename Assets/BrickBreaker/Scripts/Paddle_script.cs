using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */

    public ball_script ref_ball; // reference to our ball script
    public Gm_script ref_master_script; // reference to our gm script
    protected float speed = 8.0f; // paddle initial speed
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
        if(ref_master_script.IsDead() == false){
            if(transform.position.x >-5.5){
                if(Input.GetKey(KeyCode.LeftArrow)){
                    transform.Translate(-speed * Time.deltaTime,0,0);
                }
            }
            if(transform.position.x < 5.5){
                if(Input.GetKey(KeyCode.RightArrow)){
                    transform.Translate(speed * Time.deltaTime,0,0);
                }
            }
        }
        speed += ref_master_script.getTime()/60;
    }

    //Method that manage collision with coins
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "coin"){
            ref_ball.ScoreCoin();
        }else if(other.gameObject.tag == "speed"){
            speed += 0.5f;
        }
    }
}
