using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ennemi4Script : MonoBehaviour
{
   /*
    *
    * ATTRIBUTE
    *
    */

    // Camera Attribute
    private Camera current_camera;
    private Vector3 screen_bounds;

    // Animator references 
    private Animator ref_Ennemi_Animator; // reference to the gameObject Animator

    public  GameObject [] ref_bonus_prefab; // reference to bonus which can be drop when ennemy die
    public AudioClip [] ref_E3_clips; // ref Ennemy clips 
    private AudioSource ref_E3_audiosource; // ref ennemy audiosource
    private float speed = 1.5f;
    private Vector3 newPos;
    private int Ennemy_health = 10;
    private int attack_rate = 4;
    
    private bool is_destroy = false;
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        ref_Ennemi_Animator = GetComponent<Animator>();
        Ennemy_health = 100;

        ref_E3_audiosource = GetComponent<AudioSource>();
        ref_E3_audiosource.volume = 0.25f;

        transform.Rotate(new Vector3(0, 0, 180));

        current_camera = Camera.main;
        screen_bounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, current_camera.transform.position.z));

        Start_Shoot();
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager_script.Instance.Game_IsRunning()){
            Ennemy_movement();
            
            if(Ennemy_health <= 0 && is_destroy == false){

                // Upgrade Player point
                GameManager_script.Instance.Update_ScorePoint(500);
                GameManager_script.Instance.Decrease_Ennemy(); // Decrease Ennemy in screen

                Spawn_bonus();

                ref_Ennemi_Animator.SetTrigger("Death");
                StartCoroutine(WaitAnimation("Green_explosion",ref_Ennemi_Animator));
                is_destroy = true;
            } 
        }
    }


    /// <summary>
    ///     Method that spawn randomly differents bonus when our ennemy die
    /// </summary>
    void Spawn_bonus(){
        // Bomb bonus
        if(Random.Range(1,20) == 1){
            Instantiate(ref_bonus_prefab[0],gameObject.transform.position,Quaternion.identity);
        }
        // Health bonus
        if(Random.Range(1,20) == 1){
            Instantiate(ref_bonus_prefab[1],gameObject.transform.position,Quaternion.identity);
        }
        // Shield Bonus
        if(Random.Range(1,20) == 1){
            Instantiate(ref_bonus_prefab[2],gameObject.transform.position,Quaternion.identity);
        }
        // Double point bonus
        if(Random.Range(1,20) == 1){
            Instantiate(ref_bonus_prefab[3],gameObject.transform.position,Quaternion.identity);
        }
        // Invincibility bonus
        if(Random.Range(1,50) == 1){
            Instantiate(ref_bonus_prefab[4],gameObject.transform.position,Quaternion.identity);
        }
    }

    /// <summary>
    ///     Trigger Collions handling with player weapons
    /// </summary>
    /// <param name="other"> Collisions informations of the object</param>
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "PProjectile"){
            Ennemy_health -= 10;
        }
        if(other.gameObject.name == "MegaBombBox"){
            Ennemy_health = 0;
        }
    }

    /// <summary>
    ///     Method to handle ennemy movement in the screen
    /// </summary>
    void Ennemy_movement(){
        // Sprite transform
        if(transform.position.x > screen_bounds.x/6){
            newPos = transform.position;
            newPos.x -= speed * Time.deltaTime;
            transform.position = newPos;
        }
    }

    /// <summary>
    ///     Method to start shooting projectiles at a given attack rate
    /// </summary>
    void Start_Shoot(){
        InvokeRepeating("shoot_attack",0,attack_rate);
    }

    /// <summary>
    ///     Method to stop Shooting projectiles
    /// </summary>
    void Stop_shoot(){
        CancelInvoke("shoot_attack");
    }

    /// <summary>
    ///     Shoot one energy ball in front of the ennemy
    /// </summary>
    void shoot_attack(){
        ref_E3_audiosource.clip = ref_E3_clips[0];
        ref_E3_audiosource.Play();
        ref_Ennemi_Animator.SetTrigger("Laser");
    }


    /*
    *
    *   COROUTINE
    *
    */
    
    /// <summary>
    ///     Method called to wait the time of an given animation
    /// </summary>
    /// <param name="animation_name"> name of the target animation</param>
    /// <param name="ref_animator">reference to the animator which contain the target animation</param>
    /// <returns></returns>
    private IEnumerator WaitAnimation(string animation_name,Animator ref_animator)
    {
        if(animation_name == "Green_explosion"){
            is_destroy = true;
            ref_E3_audiosource.clip = ref_E3_clips[1];
            ref_E3_audiosource.Play(); 
        }
        yield return new WaitUntil(() => ref_animator.GetCurrentAnimatorStateInfo(0).IsName(animation_name));
        // Wait animation End
        while (ref_animator.GetCurrentAnimatorStateInfo(0).IsName(animation_name) &&
               ref_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        if(animation_name == "Green_explosion"){
            Destroy(gameObject);
        }
    }

}
