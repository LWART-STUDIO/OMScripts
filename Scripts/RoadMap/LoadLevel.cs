using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public void LoadScene()
    {
        if (SaveManager.instance.CurrentLvl > 0&& SaveManager.instance.NextLevel>0)
        {
            LoadingScreen.instance.LoadScene(SaveManager.instance.NextLevel);
            //SceneManager.LoadScene(SaveManager.instance.NextLevel);
        }
        else
        {
            LoadingScreen.instance.LoadScene(1);
          //  SceneManager.LoadScene(1);
        }
        
    }
}
