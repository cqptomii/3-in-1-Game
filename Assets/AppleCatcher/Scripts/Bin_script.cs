using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA;
using UnityEngine.Android;
public class Bin_script : MonoBehaviour
{

    /*
    *
    * ATTRIBUTE
    *
    */
    public TextMeshPro displayScore; // reference to Score TextBox
    public Spawner_script ref_spawner; // reference to spawner
    protected int score = 0 ;  //Player score
    protected float speed = 10f; //Player speed
    protected AudioSource bin_source; // reference to player audio source
    protected Animator ref_animator; // reference to Player Animator
    private Camera main_camera;
    private Vector2 screenBounds;
    private float Player_width = 0;
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
        Renderer renderer = GetComponent<Renderer>();
        Player_width = renderer.bounds.size.x / 2;

        bin_source = GetComponent<AudioSource>();
        ref_animator = GetComponent<Animator>();

        bin_source.loop = false;
        bin_source.volume = 0.7f;
    }
 
    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        float newSpeed = 0;
        // Ajouter la verification de la positin vis-à-vis de la caméra
        if(ref_spawner.GetCurrentTime() > 0){
            if(Input.GetKey(KeyCode.LeftArrow) == true){
                if(transform.position.x > -7.7){
                    newSpeed = -10f;
                    newPosition.x += Time.deltaTime* newSpeed;
                    if(newSpeed != speed){ref_animator.SetTrigger("Backward");}
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) == true){
                if(transform.position.x < 7.7){
                    newSpeed = 10f;
                    newPosition.x += Time.deltaTime*newSpeed;
                    if(speed != newSpeed) {ref_animator.SetTrigger("Forward");}
                }
            }
            else{
                if(speed != newSpeed){ref_animator.SetTrigger("Idle");}
            }
            newPosition.x = Mathf.Clamp(newPosition.x,-screenBounds.x + Player_width,screenBounds.x - Player_width);
            transform.position = newPosition;
            speed = newSpeed;
        }
        if (Input.GetKey(KeyCode.Escape) == true){
            // Retour au menu des trois jeu
        }

    }
    public int GetScore(){
        return score;
    }
    // Method to manage Collision between player and projectiles
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="projectile"){
            score++;
            displayScore.SetText("Score : " + score);
            bin_source.Play();
        }
    }
}
