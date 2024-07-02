using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music_script : MonoBehaviour
{
    /*
    *
    * ATTRIBUTE
    *
    */
    private AudioSource ref_audiosource; // reference to object audiosource
    private Animator ref_objectAnimator; // reference to object animator
    /*
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_audiosource = GetComponent<AudioSource>();
        ref_objectAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) == true){
            Application.Quit();
        }
    }

    /// <summary>
    ///     Load Apple Catcher Scene
    /// </summary>
    void Load_Apple(){
        SceneManager.LoadScene("AppleGame");
    }

    /// <summary>
    ///     Load BrickBreaker Scene
    /// </summary>
    void Load_BrickBreaker(){
        SceneManager.LoadScene("BrickGame");
    }

    /// <summary>
    ///     Load Alien Invasion Scene
    /// </summary>
    void Load_UFO(){
        SceneManager.LoadScene("UFOGame");
    }
    /// <summary>
    ///     Methoe to start animation wait Couroutine 
    /// </summary>
    public void Load_newScene(){
        StartCoroutine(WaitAnimation());
    }

    /*
    *
    *     Coroutine
    *
    */

    /// <summary>
    ///     Couroutin built to Wait end of the animation when the player click on one button in the First Menu
    /// </summary>
    /// <returns></returns>
     private IEnumerator WaitAnimation()
    {
        ref_audiosource.Play();
        yield return new WaitUntil(() => ref_objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
        // Wait animation End
        while (ref_objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed") &&
               ref_objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        Debug.Log("Animation End");
        if(gameObject.name == "Button UFO"){
            Load_UFO();
        }
        if(gameObject.name == "Button Brick"){
            Load_BrickBreaker();
        }
        if(gameObject.name == "Button Apple"){
            Load_Apple();
        }
    }
}
