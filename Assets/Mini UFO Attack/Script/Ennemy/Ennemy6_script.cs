using System.Collections;
using UnityEngine;

public class Ennemy6_script : MonoBehaviour
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
    public Animator ref_energyBall; // reference to the ball projectile Animator
    private Animator ref_laser; // reference to the Laser Animator
    
    // Audio references
    public AudioClip [] ref_Ennemy6_Audioclips;
    public  AudioSource ref_Ennemy6_AudioSource; // reference to the audio source dedicated for Laser and Death Sounds
    public AudioSource ref_Ball_Audiosource; // reference to the audiosource dedicated to EnergyBall

    public  GameObject [] ref_bonus_prefab; // reference to bonus which can be drop when ennemy die
    public GameObject ref_sprite;
    
    private float speed = 1f;
    private Vector3 newPos;
    private int Ennemy_health;
    private int attack_rate = 5;
    protected bool is_destroy = false;
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        ref_Ball_Audiosource.volume = 0.25f;
        ref_Ennemy6_AudioSource.volume = 0.25f;
        ref_laser = GetComponent<Animator>();
        Ennemy_health = 300;
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
            if(Ennemy_health <= 0 && !is_destroy){
                //Stop shoot
                Stop_shoot();
                // Upgrade Player point
                GameManager_script.Instance.Update_ScorePoint(1000);
                GameManager_script.Instance.Decrease_Ennemy(); // Decrease Ennemy in screen

                Spawn_bonus();
                ref_laser.SetTrigger("Death");
                StartCoroutine(WaitAnimation());
                is_destroy = true;
            } 
        }
    }

    /// <summary>
    ///     Trigger Collisions handling
    /// </summary>
    /// <param name="other">Collisions informations for the object</param>
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "PProjectile"){
            Ennemy_health -= 10;
        }
        if(other.gameObject.name == "MegaBombBox"){
            Ennemy_health = 0;
        }
    }

    /// <summary>
    ///     Method that handle ennemy movement
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
    ///     Start shooting projectiles at a given shoot rate
    /// </summary>
    void Start_Shoot(){
        InvokeRepeating("shoot_attack",0,attack_rate);
    }

    /// <summary>
    ///     Stop shooting projectiles
    /// </summary>
    void Stop_shoot(){
        CancelInvoke("shoot_attack");
    }

    /// <summary>
    ///     Shoot two Laser and a Ball in the scene
    /// </summary>
    void shoot_attack(){
        ref_Ball_Audiosource.Play();
        ref_Ennemy6_AudioSource.clip = ref_Ennemy6_Audioclips[0];
        ref_Ennemy6_AudioSource.Play();
        ref_laser.SetTrigger("SendLaser");
        ref_energyBall.SetTrigger("SendBall");
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
        ref_Ennemy6_AudioSource.clip = ref_Ennemy6_Audioclips[1];
        ref_Ennemy6_AudioSource.Play();
        yield return new WaitUntil(() => ref_laser.GetCurrentAnimatorStateInfo(0).IsName("explosion"));
        // Wait animation End
        while (ref_laser.GetCurrentAnimatorStateInfo(0).IsName("explosion") &&
               ref_laser.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
