using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(Input.GetKey(KeyCode.Escape) == true){
            SceneManager.LoadScene("Menu");
        }
        if(ref_master_script.IsDead() == false){
            paddle_movement();
        }
    }
    /// <summary>
    ///     Method to update the movement of our paddle
    /// </summary>
    void paddle_movement(){
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
    /// <summary>
    ///     Increase the speed of the paddle in fuction of the time from game start
    /// </summary>
    public void Increase_speed(){
        speed += ref_master_script.getTime()/120;
    }

    /// <summary>
    ///     Method to set the pos of our paddle
    ///     Use when we don't catch the ball
    /// </summary>
    /// <param name="pos"> Position required for our paddle</param>
    public void Set_Pos(Vector3 pos){
        pos.y = gameObject.transform.position.y;
        gameObject.transform.position = pos;
    }
    /// <summary>
    ///     Method that manage collision with coins
    /// </summary>
    /// <param name="other"> Collision information of the object</param>
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "coin"){
            ref_ball.ScoreCoin();
        }else if(other.gameObject.tag == "speed"){
            speed += 0.5f;
        }
    }
}
