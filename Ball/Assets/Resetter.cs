using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Resetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //	... call the Reset() function
            Reset();
        }

	}
    void Reset()
    {
        //	The reset function will Reset the game by reloading the same level
        SceneManager.LoadScene("Main");
    }
}
