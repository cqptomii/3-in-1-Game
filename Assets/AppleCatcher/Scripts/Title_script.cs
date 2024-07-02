using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Title_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown){
            StartCoroutine(LoadScene_Game());
        }
    }
    /// <summary>
    ///     Couroutine to load Game from the menu
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScene_Game(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("AppleGame");
        while( !asyncLoad.isDone){
            yield return null;
        }
    }
}
