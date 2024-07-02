using System.Collections;
using UnityEngine;
public class Gm_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */

    // Variable that store differents wall positions on the playable area
    public GameObject Left_border;
    public GameObject right_border;
    public GameObject top_border;

    public GameObject brick_prefab; // reference to a brick prefab
    public ball_script ball; // reference to our ball script
    public  Paddle_script ref_paddle; //reference to our paddle script
    public GameObject[] Spritelife; // references to health sprites
    public GameOVER ref_gameover; // reference to the gameover Screen

    protected int brick_amount = 0; // Amount of brick in the screen
    protected Renderer prefabRenderer; // prefab renderer
    protected int life_left = 3; // amount of life

    //Variable that store extremum value of playable area
    protected float x_max;
    protected float x_min;
    protected float y_max;

    protected int time_from_start = 0;
    /*
    *
    *
    *
    */

    // Start is called before the first frame update
    void Start()
    {
        Renderer Topborder_Renderer = top_border.GetComponent<Renderer>();
        Renderer Edgeborder_Renderer = Left_border.GetComponent<Renderer>();
        
        // Get the extremum of the playable area
        y_max =top_border.transform.position.y - Topborder_Renderer.bounds.size.y;
        x_min = Left_border.transform.position.x + Edgeborder_Renderer.bounds.size.x/2;
        x_max = right_border.transform.position.x - Edgeborder_Renderer.bounds.size.x/2;

        prefabRenderer = brick_prefab.GetComponent<Renderer>();
        LoadGameBoard(prefabRenderer,x_max,x_min,y_max);
        
        // Start Timer for increase game Difficulty
        StartCoroutine(StartTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // Show game Over screen
        if(life_left == 0){
            ref_gameover.ShowOverScreen();
        }
        // Load a new Level
        if(brick_amount <= 0){
            // Reset ball at it initial position
            Set_PaddlePos(ball.GetInitPosition());
            ball.resetVelocity();
            StartCoroutine(PauseCoroutine(2)); // Wait 2 seconds
            ball.Set_velocity(new Vector3(0f,-6f,0f));
            LoadGameBoard(prefabRenderer,x_max,x_min,y_max);
        }
    }

    /// <summary>
    ///     Method that fill the screen with 3 line of bricks
    /// </summary>
    /// <param name="prefab_size"> Size of the prefab</param>
    /// <param name="x_max"> x position of the right border</param>
    /// <param name="x_min"> x position of the left border</param>
    /// <param name="y_max"> y position of the top border</param>
    void FullGameBoard(Vector3 prefab_size,float x_max, float x_min, float y_max){
        Vector3 spawnPoint = new Vector3(x_min+ prefab_size.x/2,y_max - prefab_size.y/2,-0.01f);
        for(int i = 0;i < 3;i++){
            while(spawnPoint.x < x_max){
                Instantiate(brick_prefab,spawnPoint,Quaternion.identity);
                brick_amount++;
                spawnPoint.x += prefab_size.x;
            }
            spawnPoint.y -= prefab_size.y;
            spawnPoint.x = x_min + prefab_size.x/2;
        }
    }

    /// <summary>
    ///     Method that fill randomly the screen with bricks
    ///     Fill an random ammount of column and a random amount of line with boxes prefab
    /// </summary>
    /// <param name="prefab_size"> Size of the prefab</param>
    /// <param name="x_max"> x position of the right border</param>
    /// <param name="x_min"> x position of the left border</param>
    /// <param name="y_max"> y position of the top border</param>
    void RandomGameBoard(Vector3 prefab_size,float x_max, float x_min, float y_max){
        int columnAmount =  UnityEngine.Random.Range(4,6);
        Vector3 spawnPoint = new Vector3(x_min+ prefab_size.x/2,y_max - prefab_size.y/2,-0.01f);
        int lineAmount = 0;
        int instant_box = 0;
        for(int i = 0; i <= columnAmount ; i++){
            lineAmount =  UnityEngine.Random.Range(5,10);
            while(lineAmount > 0 && spawnPoint.x < x_max){
                instant_box = UnityEngine.Random.Range(0,3);
                if(instant_box==0){
                    spawnPoint.x += prefab_size.x;
                }else{
                    if(spawnPoint.x < x_max ){
                        Instantiate(brick_prefab,spawnPoint,Quaternion.identity);
                        brick_amount++;
                        spawnPoint.x += prefab_size.x;
                    }
                }
            }
            spawnPoint.y -= prefab_size.y;
            spawnPoint.x = x_min + prefab_size.x/2;
        }
    }

    /// <summary>
    ///     Method to decrease current brick on the screen
    /// </summary>
    public void Report_brickDeath(){
        brick_amount --;
    }

    /// <summary>
    ///     Method that reload a Level of bricks
    /// </summary>
    /// <param name="prefab_size"> Size of the prefab</param>
    /// <param name="x_max"> x position of the right border</param>
    /// <param name="x_min"> x position of the left border</param>
    /// <param name="y_max"> y position of the top border</param>
    void LoadGameBoard(Renderer prefabRenderer,float x_max, float x_min, float y_max){
        int prefab_choice = UnityEngine.Random.Range(0,1);
    
        if(prefab_choice == 0){
            RandomGameBoard(prefabRenderer.bounds.size,x_max,x_min,y_max);
        }else{
            FullGameBoard(prefabRenderer.bounds.size,x_max,x_min,y_max);
        }
    }

    /// <summary>
    ///     Method which decrease player health and hide associated sprite
    /// </summary>
    public void decrease_heath(){
        life_left--;
        Renderer ob_renderer = Spritelife[life_left].GetComponent<Renderer>();
        ob_renderer.enabled = false;
    }
    
    /// <summary>
    ///     Method which return time attribute
    /// </summary>
    /// <returns> time last since the game start</returns>
    public int getTime(){
        return time_from_start;
    }

    /// <summary>
    ///     Method that look if we have life left
    /// </summary>
    /// <returns> true : life left <= 0 / false otherwise </returns>
    public bool IsDead(){
        if(life_left >0){
            return false;
        }else{
            return true;
        }
    }

    /// <summary>
    ///     Method to set the position of the paddle
    /// </summary>
    /// <param name="pos"> position Vector </param>
    public void Set_PaddlePos(Vector3 pos){
        ref_paddle.Set_Pos(pos);
    }

    /*
    *
    *   Coroutine 
    *
    */

    /// <summary>
    ///     Coroutine that init a Timer which will set the speed of the ball
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTimer()
    {

        while(time_from_start < 120){
            yield return new WaitForSeconds(1f);
            if(time_from_start %10 == 0){
                ball.UpVelocity();
                ref_paddle.Increase_speed();
            }
            time_from_start++;
        }
    }

    /// <summary>
    ///     Make a pause in the game - use when the player don't catch the ball
    /// </summary>
    /// <param name="seconds"> amount of seconds paused</param>
    /// <returns></returns>
    private IEnumerator PauseCoroutine(float seconds)
    {
        // Initial state
        float originalTimeScale = Time.timeScale;

        // Pause
        Time.timeScale = 0f;

        // Waite the amount of second
        float pauseEndTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        // Unpause
        Time.timeScale = originalTimeScale;
    }
}
