using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene_script : MonoBehaviour
{

    /**
    *
    *ATTRIBUTE
    *
    */
    private Animator ref_objectAnimator; // reference to current object animator
    private Scene current_scene; // reference to the current scene Loaded
    /**
    *
    *
    *
    */
    // Start is called before the first frame update
    void Start()
    {
        ref_objectAnimator = GetComponent<Animator>();
        current_scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    ///     Method to start the animation wait Coroutine
    /// </summary>
    public void Load_newScene(){
        StartCoroutine(WaitAnimation());
    }

    /*
    *
    *   Coroutine
    *
    */

    /// <summary>
    ///     Coroutine built to wait the time of the animation Pressed on the GameOver Menu
    /// </summary>
    /// <returns></returns>
     private IEnumerator WaitAnimation()
    {
        yield return new WaitUntil(() => ref_objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed"));
        // Wait animation End
        while (ref_objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pressed") &&
               ref_objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        if(gameObject.name == "Quit"){
            SceneManager.LoadScene("Menu");
        }else if(gameObject.name == "Replay"){
            SceneManager.LoadScene(current_scene.name);
        }
    
    }
}
