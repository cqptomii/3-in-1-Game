using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Jobs;

public class Player_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */
    public GameObject projectile_prefab; // reference to player projectiles prefab
    public Bomb_script ref_BombScript; // reference to Bomb script

    protected Vector2 Projectile_spawnPos;
    protected Animator ref_animator; // reference to the player animator
    protected float speed = 7f; // initial player  movement speed
    private Camera mainCamera; // reference to our game camera
    private Vector2 screenBounds; // Camera Bounds
    private float UI_pos = 3.7f; // Y coordinate of Main UI of the game
    private float Player_height = 0; // height of player sprite
    private float Player_width = 0; // width of the player sprite 
    private float Player_health = 100f;
    private bool is_shooting = false;

    /*
    *
    *   
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        Renderer renderer = GetComponent<Renderer>();
        Player_width = renderer.bounds.size.x / 2;
        Player_height = renderer.bounds.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition.y += Time.deltaTime * speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition.y -= Time.deltaTime * speed;
        }
        if(Input.GetKeyDown(KeyCode.Space) == true && !is_shooting){
            Start_shoot();
        }
        if(Input.GetKeyUp(KeyCode.Space) == true && is_shooting){
            Stop_shoot();
            ref_animator.SetTrigger("Normal");
            
        }

        if(Input.GetKey(KeyCode.B) == true && ref_BombScript.CanUse()){
            ref_animator.SetTrigger("Bomb");
            ref_BombScript.UseBomb();
            //Special Bomb
        }

        if(transform.position != newPosition){
            // Limiter la position de l'objet pour qu'il reste dans les limites de l'écran
            newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + Player_width , screenBounds.x - Player_width);
            newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + Player_height, UI_pos - Player_height);

            transform.position = newPosition;
        }
    }
    // Methode to handle Collision between our player and other objects
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "enemy" || other.gameObject.tag =="eBullet"){
            ref_animator.SetTrigger("Damaged");

            //Get damage
        }
        if(other.gameObject.tag == "bonusB"){
            ref_BombScript.gainBomb();
        }
    }

    //This method provide current player rounded health 
    public int GetInt_Health(){
        return Mathf.RoundToInt(Player_health);
    }

    // This method decrease an amount of Player health 
    void Decrease_health(float amount){
        Player_health -= amount;
        Player_health = Mathf.Clamp(Player_health,0,100);
    }

    // This method restore an amount of Player health 
    void Gain_health(float amount){
        Player_health += amount;
        Player_health = Mathf.Clamp(Player_health,0,100);
    }

    // This method Start projectile shooting
    void Start_shoot(){
        is_shooting = true;
        InvokeRepeating("ShootProjectile",0f,0.12f);
    }

    // This method Stop projectile shooting
    void Stop_shoot(){
        is_shooting = false;
        CancelInvoke("ShootProjectile");
    }

    // This method Instantiate two projectile in front of the player
    void ShootProjectile()
    {
        // Shoot laser
        ref_animator.SetTrigger("Shoot");
        Projectile_spawnPos = new Vector2(transform.position.x + 1.1f,transform.position.y);
        Instantiate(projectile_prefab,Projectile_spawnPos,Quaternion.identity);
        Projectile_spawnPos.y -= 0.25f;
        Instantiate(projectile_prefab,Projectile_spawnPos,Quaternion.identity);
    }
}
