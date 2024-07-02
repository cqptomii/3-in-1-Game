using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
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
    private Camera main_camera; // reference to the Scene Camera
    private Vector2 screenBounds; // Viewing frustrum of the camera on the far plane
    private float Player_width = 0; // width of the player sprite
    public GameOVER ref_gameOver; // reference to GameOver script
    private Vector3 newPosition;
    
    /*
    *
    * 
    *
    */


    // Start is called before the first frame update
    void Start()
    {
        // Get Camera viewing caracteristics
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
        if(Input.GetKey(KeyCode.Escape) == true){
            SceneManager.LoadScene("Menu");
        }
        if(ref_spawner.GetCurrentTime() > 0){
            player_movement();
        }
        if (Input.GetKey(KeyCode.Escape) == true){
            SceneManager.LoadScene("Menu");
        }

    }
    /// <summary>
    ///     Method to handle player position on the screen
    ///     Update animation with the current player state : Idle / Backward / Forward
    /// </summary>
    void player_movement(){
        float newSpeed = 0;
        newPosition = transform.position;
        if(Input.GetKey(KeyCode.LeftArrow) == true){
                newSpeed = -10f - 20/ref_spawner.GetCurrentTime();
                newPosition.x += Time.deltaTime* newSpeed;
                if(newSpeed == speed){ref_animator.SetTrigger("Backward");}
            }
            else if (Input.GetKey(KeyCode.RightArrow) == true){
                newSpeed = 10f + 20/ref_spawner.GetCurrentTime();
                newPosition.x += Time.deltaTime*newSpeed;
                if(speed == newSpeed) {ref_animator.SetTrigger("Forward");}
            }else{
                if(speed == 0){ref_animator.SetTrigger("Idle");}
            }
            
            //Update the player position in the viewing frustrum of the camera
            newPosition.x = Mathf.Clamp(newPosition.x,-screenBounds.x + Player_width,screenBounds.x - Player_width);
            transform.position = newPosition;
            speed = newSpeed;
    }

    /// <summary>
    ///         Method to get the current score of the player
    /// </summary>
    /// <returns></returns>
    public int GetScore(){
        return score;
    }



    /// <summary>
    ///     Method to handle collision between player and differents Apples
    /// </summary>
    /// <param name="other"> Collisions details of the object</param>
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="projectile"){
            score++;
            displayScore.SetText("Score : " + score);
            bin_source.Play();
        }
        else if (other.gameObject.tag == "otherProject")
        {
            if (other.gameObject.name == "Rotten_prefab(Clone)")
            {
                score--;
                displayScore.SetText("Score : " + score);
                bin_source.Play();
            }
            else if (other.gameObject.name == "Gold_prefab(Clone)")
            {
                score = score +3;
                displayScore.SetText("Score : " + score);
                bin_source.Play();
            }
            else if (other.gameObject.name == "Bomb_prefab(Clone)")
            {
                //When you catch a bomb show GameOver Screen
                bin_source.Play();
                ref_spawner.currentTime = 0;
                ref_gameOver.ShowOverScreen();
            }
        }
    }
}
