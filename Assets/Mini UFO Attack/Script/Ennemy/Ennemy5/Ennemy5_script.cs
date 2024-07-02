using System.Collections;
using UnityEngine;

public class Ennemy5_script : MonoBehaviour
{
    /*
    *
    *    ATTRIBUTE
    *
    */
    public GameObject ref_ball_prefab;
    public AudioClip [] ref_ennemyClips;
    public GameObject [] ref_bonus;

    private AudioSource ref_audioSource; // reference to gameObject audioSource
    private Animator ref_animator; // reference to gameObject Animator
    
    private Camera current_cam;
    private Vector3 Screen_bounds;

    private Vector3 newPos;
    private int Ennemy_health;
    private int laser_rate; // Spawn rate of the laser
    private int ball_rate; // spawn rate of projectiles
    private float current_speed; // ennemy speed
    private bool is_destroy = false;

    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(180,0,180));

        current_cam = Camera.main;
        Screen_bounds = current_cam.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,current_cam.transform.position.z));

        ref_audioSource = GetComponent<AudioSource>();
        ref_audioSource.volume = 0.25f;

        ref_animator = GetComponent<Animator>();

        Ennemy_health = 100;
        laser_rate = 6;
        ball_rate = 3;
        current_speed = 1f;

        Start_Laser();
        Start_ball();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager_script.Instance.Game_IsRunning()){
            Ennemy_movement();
            if(Ennemy_health <= 0 && !is_destroy){
                Stop_ball();
                stop_Laser();

                GameManager_script.Instance.Update_ScorePoint(500);
                GameManager_script.Instance.Decrease_Ennemy();

                Spawn_bonus();
                ref_animator.SetTrigger("Death");
                StartCoroutine(WaitAnimation());
            }
        }
    }
    
    /// <summary>
    ///     Trigger Collisions handling with Player weapons
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
    ///     Method that handle the movement of the gameObject
    /// </summary>
    void Ennemy_movement(){
        if(transform.position.x > Screen_bounds.x/3){
            newPos = transform.position;
            newPos.x -= current_speed * Time.deltaTime;
            transform.position = newPos;
        }
    }

    /// <summary>
    ///     Method that spawn randomly differents bonus when our ennemy die
    /// </summary>
    void Spawn_bonus(){
        if(UnityEngine.Random.Range(1,20) == 1){
            Instantiate(ref_bonus[0],gameObject.transform.position,Quaternion.identity);
        }
        // Health bonus
        if(UnityEngine.Random.Range(1,20) == 1){
            Instantiate(ref_bonus[1],gameObject.transform.position,Quaternion.identity);
        }
        // Shield Bonus
        if(UnityEngine.Random.Range(1,20) == 1){
            Instantiate(ref_bonus[2],gameObject.transform.position,Quaternion.identity);
           }
        // Double point bonus
        if(UnityEngine.Random.Range(1,20) == 1){
             Instantiate(ref_bonus[3],gameObject.transform.position,Quaternion.identity);
        }
        // Invincibility bonus
        if(UnityEngine.Random.Range(1,50) == 1){
            Instantiate(ref_bonus[4],gameObject.transform.position,Quaternion.identity);
        }  
    }

    /// <summary>
    ///     Method which Start shooting projectile at a given shoot rate
    /// </summary>
    void Start_ball(){
        InvokeRepeating("shoot_ball",0f,ball_rate);
    }

    /// <summary>
    ///     Method which Stop shooting projectile
    /// </summary>
    void Stop_ball(){
        CancelInvoke("shoot_ball");
    }

    /// <summary>
    ///     Method which currently shoot two new projectiles in the scene
    /// </summary>
    void shoot_ball(){
        ref_audioSource.clip = ref_ennemyClips[2];
        ref_audioSource.Play();
        Instantiate(ref_ball_prefab,gameObject.transform.position,Quaternion.identity);
        Instantiate(ref_ball_prefab,gameObject.transform.position,Quaternion.identity);
    }

    /// <summary>
    ///     Method that start Laser shooting at a given rate
    /// </summary>
    void Start_Laser(){
        InvokeRepeating("shoot_Laser",0f,laser_rate);
    }

    /// <summary>
    ///     Method that stop shooting Laser
    /// </summary>
    void stop_Laser(){
        CancelInvoke("shoot_Laser");
    }

    /// <summary>
    ///     Shoot one Laser from the gameObject 
    /// </summary>
    void shoot_Laser(){
        ref_audioSource.clip = ref_ennemyClips[0];
        ref_audioSource.Play();
        ref_animator.SetTrigger("Laser");
    }
    /*
    *
    * Coroutine
    *
    */

    /// <summary>
    ///     Wait end of the animation "Death" - Use when the ennemy health reach 0
    /// </summary>
    /// <returns></returns>
     private IEnumerator WaitAnimation()
    {
        is_destroy = true;
        ref_audioSource.clip = ref_ennemyClips[1];
        ref_audioSource.Play();
        yield return new WaitUntil(() => ref_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        // Wait animation End
        while (ref_animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
               ref_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
