using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy2_script : MonoBehaviour
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
    private Animator ref_ball;

    // Audio references
    public AudioSource ref_Ball_Audiosource; // reference to the audiosource dedicated to EnergyBall
    public AudioClip ref_Explosion_Audioclips;
    public AudioSource ref_Explosion_AudioSource; // reference to the audio source dedicated for Death Sounds

    public GameObject[] ref_bonus_prefab; // reference to bonus which can be drop when ennemy die
    private float speed = 1f;
    private Vector3 newPos;
    private int Ennemy_health = 10;
    private int attack_rate = 5;
    protected bool is_destroy = false;
    private float timer = 0f; //Timer tp track elapsed time
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        ref_Ball_Audiosource.volume = 0.25f;
        ref_Explosion_AudioSource.volume = 0.25f;
        ref_ball = GetComponent<Animator>();

        Ennemy_health = 50;

        current_camera = Camera.main;
        screen_bounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, current_camera.transform.position.z));

        Start_Shoot();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager_script.Instance.Game_IsRunning())
        {
            timer += Time.deltaTime;
            if(timer >= 3f)
            {
                StartCoroutine(StopAndGo());
            }
            else
            {
                Ennemy_movement();
                if (Ennemy_health <= 0 && !is_destroy)
                {
                    //Stop shoot
                    Stop_shoot();
                    // Upgrade Player point
                    GameManager_script.Instance.Update_ScorePoint(500);
                    GameManager_script.Instance.Decrease_Ennemy(); // Decrease Ennemy in screen

                    Spawn_bonus();
                    ref_ball.SetTrigger("Death");
                    StartCoroutine(WaitAnimation());
                    is_destroy = true;
                }
            }
            
        }
    }

    /// <summary>
    ///     Trigger Collision handling
    /// </summary>
    /// <param name="other"> Collision informations for the object</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PProjectile")
        {
            Ennemy_health -= 10;
        }
        if (other.gameObject.name == "MegaBombBox")
        {
            Ennemy_health = 0;
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
    ///     Start shooting projectile at a given shoot rate
    /// </summary>
    void Start_Shoot()
    {
        InvokeRepeating("shoot_attack", 0, attack_rate);
    }

    /// <summary>
    ///     Stop projectile shooting
    /// </summary>
    void Stop_shoot()
    {
        CancelInvoke("shoot_attack");
    }

    /// <summary>
    ///     Method which sent a energyBall in the screen 
    /// </summary>
    void shoot_attack()
    {
        ref_Ball_Audiosource.Play();
        ref_ball.SetTrigger("SendBall");
    }

    /// <summary>
    ///     Method that handle ennemy movement in the screen
    /// </summary>
    void Ennemy_movement()
    {
        newPos = transform.position;
        newPos.x -= speed * Time.deltaTime;
        transform.position = newPos;
    }


    /*
    *
    * Coroutine
    *
    */

    /// <summary>
    ///     Stop the ennemy for 1 second, then restart the movement of the ennemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopAndGo()
    {
        speed = 0f; // Stop the enemy
        yield return new WaitForSeconds(1f); // Wait for 1 second
        speed = 1f;
        timer = 0f;
    }

    /// <summary>
    ///     Wait end of the animation "Death" - Use when the ennemy health reach 0
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAnimation()
    {
        ref_Explosion_AudioSource.clip = ref_Explosion_Audioclips;
        ref_Explosion_AudioSource.Play();
        yield return new WaitUntil(() => ref_ball.GetCurrentAnimatorStateInfo(0).IsName("explosion"));
        // Wait animation End
        while (ref_ball.GetCurrentAnimatorStateInfo(0).IsName("explosion") &&
               ref_ball.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
