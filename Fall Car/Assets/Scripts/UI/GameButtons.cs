using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    

    public void PlayAgain()
    {
        //show cool load screen (make scene manager)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
