using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }

}
