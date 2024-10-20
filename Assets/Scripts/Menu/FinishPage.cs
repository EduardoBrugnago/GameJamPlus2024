using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPage : MonoBehaviour
{
 
    void Update()
    {
        if (Input.anyKeyDown)
        {
            ControlerLevels controlerLevels = FindFirstObjectByType<ControlerLevels>();
            if(controlerLevels != null )
            {
                controlerLevels.LoadNextScene();
            }
        }
    }
}
