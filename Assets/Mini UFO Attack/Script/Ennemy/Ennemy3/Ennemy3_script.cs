using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Ennemy3_script : MonoBehaviour
{
    /*
    *
    *   ATTRIBUTES
    *
    */
    public GameObject ref_ball_prefab; // ref projectile prefab
    public GameObject [] ref_bonus_prefab; // ref bonus prefabs
    public AudioClip [] ref_E3_clips; // ref Ennemy clips 
    private AudioSource ref_E3_audiosource; // ref ennemy audiosource
    public Animator ref_ball_animator; // ref projectile animator
    public Animator ref_death_animator; // ref ennemy animator
    private int current_health = 1; // ennemy health
    private int speed; // ennemy speed
    private int attack_rate = 4; // projectile rate
    private bool is_destroy = false;
    private Vector2 NewPos;
    public GameObject ref_Ball_spawn; // reference to the projectile spawn point
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0,0,180));

        ref_E3_audiosource = GetComponent<AudioSource>();
        ref_E3_audiosource.volume = 0.25f;

        current_health = 200;
        speed = Random.Range(1,3);

        Start_Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager_script.Instance.Game_IsRunning()){
            Ennemy_movement();
            if(current_health <= 0 && !is_destroy){
                //Stop shoot
                Stop_Shoot();
                // Upgrade Player point
                GameManager_script.Instance.Update_ScorePoint(400);
                GameManager_script.Instance.Decrease_Ennemy(); // Decrease Ennemy in screen

                Spawn_bonus();
                ref_death_animator.SetTrigger("Death");
                StartCoroutine(WaitAnimation("Green_explosion",ref_death_animator));
                is_destroy = true;
            } 
        }
    }
    /// <summary>
    ///     Method to catch Trigger Collision with our ennemy
    /// </summary>
    /// <param name="other">Collisions informations of the object</param>
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "PProjectile"){
            current_health -= 10;
        }
        if(other.gameObject.tag == "MegaBomb"){
            current_health = 0;
        }
    }

    /// <summary>
    ///     Method to handle ennemy movement in the screen
    /// </summary>
    void Ennemy_movement(){
        if(gameObject.transform.position.x > 0){
            NewPos = gameObject.transform.position;
            NewPos.x -= speed * Time.deltaTime;
            gameObject.transform.position = NewPos;
        }
    }
    
    /// <summary>
    ///     Method to start shooting projectiles at a given attack rate
    /// </summary>
    void Start_Shoot(){
        InvokeRepeating("Shoot_Ball",attack_rate,attack_rate);
    }
    
    /// <summary>
    ///     Method to stop Shooting projectiles
    /// </summary>
    void Stop_Shoot(){
        CancelInvoke("Shoot_Ball");
    }

    /// <summary>
    ///     Method that spawn a projetile in front of the ennemy
    /// </summary>
    void Shoot_Ball(){
        ref_E3_audiosource.clip = ref_E3_clips[0];
        ref_E3_audiosource.Play();
        ref_ball_animator.SetTrigger("Ball");
        StartCoroutine(WaitAnimation("Ball",ref_ball_animator));
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
        if(animation_name == "Ball"){
            ref_animator.SetTrigger("Normal");
            Instantiate(ref_ball_prefab,ref_Ball_spawn.transform.position,Quaternion.identity);
        }
    }

}
