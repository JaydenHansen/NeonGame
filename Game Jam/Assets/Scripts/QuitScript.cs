using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class QuitScript : MonoBehaviour {

    // Use this for initialization
   public  void ButtonClicked(int buttonNo)
    { 
        
        Application.Quit();
    }
}
