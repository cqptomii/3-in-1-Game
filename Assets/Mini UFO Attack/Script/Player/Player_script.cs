using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTE
    *
    */

    //References & prefab
    public GameObject projectile_prefab; // reference to player projectiles prefab
    public Bomb_script ref_BombScript; // reference to Bomb script
    public Animator ref_Bombanimator; // reference to the Bomb animator
    public GameOVER ref_GameOver_script; // reference to the game over Script
    public Game_pause_script ref_GamePause_script; // reference to the Pause Menu
    public Wiki_script ref_wiki_script; // reference to the wiki script

    // Reference to every audioClips related with the player
    // 
    //  0 - Laser Shoot  / 1 - Player damage 
    //  2 - Mega Bomb / 3 -  Player Death 
    //  5/6 Player explode
    public AudioClip [] ref_Player_Audioclip;
    public AudioSource ref_Player_AudioSource;
    public AudioSource ref_bonus_AudioSource;

    // Environment variable
    protected float speed = 7f; // initial player  movement speed
    private Camera current_camera;
    private Vector2 screenBounds; // Camera Bounds
    private float UI_pos = 3.7f; // Y coordinate of Main UI of the game

    //Player Attribute
    protected Vector2 Projectile_spawnPos;
    private Animator ref_playerAnimator; // reference to the player Animator
    private float Player_height = 0; // height of player sprite
    private float Player_width = 0; // width of the player sprite 
    private bool is_shooting = false;
    private bool is_bomb = false;
    private bool is_damaged = false;
    /*
    *
    *   
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        current_camera = Camera.main;
        screenBounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,current_camera.transform.position.z));

        ref_Player_AudioSource.volume = 0.25f;
        ref_bonus_AudioSource.volume = 0.5f;

        ref_playerAnimator = GetComponent<Animator>();
        Renderer renderer = GetComponent<Renderer>();

        Player_width = renderer.bounds.size.x / 2;
        Player_height = renderer.bounds.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager_script.Instance.Game_IsRunning()){
            // Return to the Main menu
            if(Input.GetKey(KeyCode.Escape) == true && ref_wiki_script.Get_wiki_state() == false && ref_GamePause_script.Get_Opening() == false){
                if(ref_GamePause_script.Get_State() == true){
                    ref_GamePause_script.Game_Pause();
                }else{
                    ref_GamePause_script.Game_Continue();
                }
            }
            Player_Movement();
            Player_Shoot();
            Player_State();
            Player_IsUse_Bomb();
        }
    }
    
    /// <summary>
    ///     Handle player movement 
    /// </summary>
    void Player_Movement(){
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

        // Update current player position only if the player move
        if(transform.position != newPosition){
            // Limiter la position de l'objet pour qu'il reste dans les limites de l'écran
            newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + Player_width , screenBounds.x - Player_width);
            newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + Player_height, UI_pos - Player_height);

            transform.position = newPosition;
        }
    }

    /// <summary>
    ///     Method to verify if the player is dead or alive
    /// </summary>
    void Player_State(){
        //Player Death Handle
        if(!GameManager_script.Instance.Is_Alive()){
            ref_Player_AudioSource.clip = ref_Player_Audioclip[3];
            ref_Player_AudioSource.Play();
            ref_playerAnimator.SetTrigger("Death");
            StartCoroutine(WaitPlayerAnimation("Death"));
        }
        // Standard State 
        if(is_damaged == false && GameManager_script.Instance.Is_Alive()){
                ref_playerAnimator.SetTrigger("Normal");
        }
    }

    /// <summary>
    ///     Handle player MegaBomb use
    /// </summary>
    void Player_IsUse_Bomb(){
        // Handle Player MegaBomb
        if(Input.GetKey(KeyCode.B) == true && ref_BombScript.CanUse() && !is_bomb){
                //Special Bomb
                is_bomb = true;
                ref_Player_AudioSource.clip = ref_Player_Audioclip[2];
                ref_Player_AudioSource.Play();
                ref_Bombanimator.SetTrigger("Bomb");
                ref_BombScript.UseBomb();
                StartCoroutine(WaitBombAnimation());
        }
    }

    /// <summary>
    ///     Handle player shoot projectiles
    /// </summary>
    void Player_Shoot(){ 
        if(Input.GetKeyDown(KeyCode.Space) == true && !is_shooting){
            Start_shoot();
            ref_playerAnimator.SetBool("IsMuted",true);
        }
        if(Input.GetKeyUp(KeyCode.Space) == true && is_shooting){
            Stop_shoot();
            ref_playerAnimator.SetBool("IsMuted",false);
        }
    }

    /// <summary>
    ///     This method Start projectile shooting at a given rate
    /// </summary>
    void Start_shoot(){
        is_shooting = true;
        InvokeRepeating("ShootProjectile",0f,0.12f);
    }

    /// <summary>
    ///     This method Stop projectile shooting
    /// </summary>
    void Stop_shoot(){
        is_shooting = false;
        CancelInvoke("ShootProjectile");
    }

    /// <summary>
    ///     This method Instantiate two projectile in front of the player
    /// </summary>
    void ShootProjectile()
    {
        // Shoot laser
        ref_Player_AudioSource.clip = ref_Player_Audioclip[0];
        ref_Player_AudioSource.Play();
        ref_playerAnimator.SetTrigger("Shoot");
        Projectile_spawnPos = new Vector2(transform.position.x + 1.1f,transform.position.y);
        Instantiate(projectile_prefab,Projectile_spawnPos,Quaternion.identity);
        Projectile_spawnPos.y -= 0.25f;
        Instantiate(projectile_prefab,Projectile_spawnPos,Quaternion.identity);
    }
    
    /// <summary>
    ///     Trigger Collisions handling
    /// </summary>
    /// <param name="collision">Collisions informations for the object</param>
    void OnTriggerEnter2D(Collider2D collision){
        if(!is_bomb){
            if(collision.gameObject.tag == "bonusB" ){
                ref_BombScript.gainBomb();
                ref_bonus_AudioSource.Play();
            }
            if(collision.gameObject.name == "Powerup_Health(Clone)"){
                ref_bonus_AudioSource.Play();
                GameManager_script.Instance.Gain_health(20);
            }
            if(collision.gameObject.name == "PowerUp_Shield(Clone)"){
                ref_bonus_AudioSource.Play();
                GameManager_script.Instance.ShieldStart();
            }
            if(collision.gameObject.name == "Powerup_invincibility(Clone)"){
                ref_bonus_AudioSource.Play();
                GameManager_script.Instance.Invicibility_Start();
            }
            if(collision.gameObject.name == "Powerup_x2(Clone)"){
                ref_bonus_AudioSource.Play();
                GameManager_script.Instance.Double_pointStart();
            }
        }
        if(collision.gameObject.tag == "Ennemy" || collision.gameObject.tag =="EProjectile" || collision.gameObject.tag == "BlueBall"  || collision.gameObject.tag == "PurpleBall" || collision.gameObject.tag == "Laser"){
            if(!is_bomb){
                Handle_Ennemy(collision,collision.gameObject.tag);
            }
        }
    }

    /// <summary>
    ///     Handle player hit ennemy projectiles
    /// </summary>
    /// <param name="other"> Collisions informations for the object</param>
    /// <param name="tag"> tag of the gameObject who collide our player</param>
    void Handle_Ennemy(Collider2D other,String tag){
        if(!GameManager_script.Instance.GetInvicibilityState()){
            if(GameManager_script.Instance.GetShieldState()){
                GameManager_script.Instance.ShieldStop();
            }else{
                Debug.Log(other.gameObject.name);
                ref_Player_AudioSource.clip = ref_Player_Audioclip[1];
                ref_Player_AudioSource.Play();
                is_damaged = true;
                ref_playerAnimator.SetTrigger("Damaged");
                StartCoroutine(WaitPlayerAnimation("Damaged")); //Wait animation

                // Handle point decrease in fuction of Ennemy type
                if(tag == "Ennemy"){
                    GameManager_script.Instance.Decrease_health(5f);
                }else if(tag == "EProjectile"){
                    GameManager_script.Instance.Decrease_health(7.5f);
                }else if(tag == "Laser"){
                    GameManager_script.Instance.Decrease_health(5f);
                }else if(tag == "BlueBall"){
                    GameManager_script.Instance.Decrease_health(2f);
                }else if(tag == "PurpleBall"){
                    GameManager_script.Instance.Decrease_health(4f);
                }
                is_damaged = false;
            }
        }
    }


    /*
    *
    * Coroutine
    *
    */

    /// <summary>
    ///     Coroutin which wait the end of the animation MegaBomb 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitBombAnimation()
    {
        yield return new WaitUntil(() => ref_Bombanimator.GetCurrentAnimatorStateInfo(0).IsName("MegaBomb"));
        // Wait animation End
        while (ref_Bombanimator.GetCurrentAnimatorStateInfo(0).IsName("MegaBomb") &&
               ref_Bombanimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        is_bomb = false;
    }

    /// <summary>
    ///     Coroutine which wait the end of a given animation from the ref_playerAnimator
    /// </summary>
    /// <param name="animation_name"> name of the animation</param>
    /// <returns></returns>
    private IEnumerator WaitPlayerAnimation(String animation_name)
    {
        yield return new WaitUntil(() => ref_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animation_name));
        // Wait animation End
        while (ref_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animation_name) &&
               ref_playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Debug.Log("Animation End");
        if(animation_name == "Death"){
            ref_GameOver_script.ShowOverScreen();
        }
    }
}
