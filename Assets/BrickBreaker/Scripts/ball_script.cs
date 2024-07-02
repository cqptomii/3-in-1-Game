using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
public class ball_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    public Gm_script ref_master; // reference to master script
    public TextMeshPro score_text; // reference to Score Text Box
    public AudioClip[] clip_array; // Sound Array

    protected Vector3 force = new Vector3(0f,-6f,0f); // Force appliquer initialement à la balle
    protected Rigidbody2D rb;  // reference to the ball rigid Body
    protected AudioSource Sound_source; // Ball Audiosource
    protected int score = 0; // Player Score
    protected Vector3 init_pos; // inital position of the ball
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position;

        Sound_source = GetComponent<AudioSource>();
        Sound_source.loop = false;
        Sound_source.volume = 0.7f;

        rb = GetComponent<Rigidbody2D>();
        if (rb){
            rb.velocity = force;
        }
        else{
            Debug.LogError("No Rigidbody attached to the object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
            // Player don't catch the ball
        if(transform.position.y < -5 && !ref_master.IsDead()){
            Sound_source.clip = clip_array[3];
            Sound_source.Play();
            
            //Update Score
            score -= 500;
            score_text.SetText("Score : " + score);
            ref_master.decrease_heath(); //update health

            // Reset ball at it initial position
            transform.position = init_pos;
            ref_master.Set_PaddlePos(init_pos);
            resetVelocity();
            StartCoroutine(PauseCoroutine(2)); // Wait 2 seconds
            rb.velocity = force;
        }
    }
    /// <summary>
    ///     Method to handle collision between our ball and other objects in the scene
    ///     (box / wall / pad)
    /// </summary>
    /// <param name="other">Collision informations of the object</param>
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "box"){
            score += 50;
            score_text.SetText("Score : " + score);
            Sound_source.clip = clip_array[0];
            Sound_source.Play();
            ref_master.Report_brickDeath();
        }
        if(other.gameObject.tag == "wall"){
            Sound_source.clip = clip_array[1];
            Sound_source.Play();
        }
        if(other.gameObject.tag == "pad"){
            float diffX = other.transform.position.x - transform.position.x;
            rb.velocity += new Vector2(diffX*3,0.1f);
            Sound_source.clip = clip_array[2];
            Sound_source.Play();
        }
    }

    /// <summary>
    ///     Method that score point in case of coins catch
    /// </summary>
    public void ScoreCoin(){
        score += 500;
        score_text.SetText("Score:"+ score);
    }

    /// <summary>
    ///     Method to provide current Score to
    /// </summary>
    /// <returns> the current score of the player </returns>
    public int getScore(){
        return score;
    }
    public Vector3 GetInitPosition(){
        return init_pos;
    }
    public void Set_velocity(Vector2 force){
        rb.velocity = force;
    }

    /// <summary>
    ///     Reset the velocity associated with the ball rigidBody to freeze the ball
    /// </summary>
    public void resetVelocity(){
        rb.velocity = new Vector2(0,0);
    }

    /// <summary>
    ///     Upgrade the ball velocity in function of the time in order to make more difficulty
    /// </summary>
    public void UpVelocity(){
        rb.velocity += new Vector2(ref_master.getTime()/200,ref_master.getTime()/200);
    }

    /*
    *
    *   Coroutine
    *
    */

    /// <summary>
    ///     Make a pause in the game - use when the player don't catch the ball
    /// </summary>
    /// <param name="seconds"> amount of seconds paused</param>
    /// <returns></returns>
    private IEnumerator PauseCoroutine(float seconds)
    {
        // Initial state
        float originalTimeScale = Time.timeScale;

        // Pause
        Time.timeScale = 0f;

        // Waite the amount of second
        float pauseEndTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        // Unpause
        Time.timeScale = originalTimeScale;
    }
}
