using System.Collections;
using UnityEngine;
public class Ennemi1_script : MonoBehaviour
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
    private Animator ref_animator; // reference to Ennemi 1 Animator

    // Audio references
    private AudioSource ref_Explosion_AudioSource; // reference to the audio source dedicated for Death Sounds

    private float speed = 1f; // ennemy normal speed
    private float fastSpeed = 10f; // ennemy fast speed
    private Vector3 newPos;
    private int Ennemy_health = 1;
    protected bool is_destroy = false;
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0f,0f,180f));

        ref_animator = GetComponent<Animator>();
        ref_Explosion_AudioSource = GetComponent<AudioSource>();
        ref_Explosion_AudioSource.volume = 0.25f;

        Ennemy_health = 10;

        current_camera = Camera.main;
        screen_bounds = current_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, current_camera.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager_script.Instance.Game_IsRunning())
        {
            // Check if the enemy has reached approximately the x position of 7.5
            if (Mathf.Abs(transform.position.x - 7.5f) < 0.1f)
            {
                StartCoroutine(StopAndGo());
            }
            if (transform.position.x < -screen_bounds.x)
            {
                Destroy(gameObject);
            }
            Ennemy_movement();
            if (Ennemy_health <= 0 && !is_destroy)
            {
                StopCoroutine(StopAndGo());
                is_destroy = true;
                Debug.Log(is_destroy);
                // Upgrade Player point
                GameManager_script.Instance.Update_ScorePoint(100);
                GameManager_script.Instance.Decrease_Ennemy(); // Decrease Ennemy in screen
                ref_animator.SetTrigger("Death");
                StartCoroutine(WaitAnimation());
            }
        }
    }

    /// <summary>
    ///     Trigger Collisions handling with player weapons
    /// </summary>
    /// <param name="other">Collisions informations of the object</param>
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
    ///     Method that handle ennemy movement
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
    ///     Stop the gameObject for 1 seconds and then increase his speed to fastSpeed
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopAndGo()
    {
        speed = 0f; // Stop the enemy
        yield return new WaitForSeconds(1f); // Wait for 1 second
        speed = fastSpeed; // Increase the speed
        Ennemy_movement();
    }

    /// <summary>
    ///     Wait end of the animation "Death" - Use when the ennemy health reach 0
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAnimation()
    {
        ref_Explosion_AudioSource.Play();
        yield return new WaitUntil(() => ref_animator.GetCurrentAnimatorStateInfo(0).IsName("explosion"));
        // Wait animation End
        while (ref_animator.GetCurrentAnimatorStateInfo(0).IsName("explosion") &&
               ref_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

}
