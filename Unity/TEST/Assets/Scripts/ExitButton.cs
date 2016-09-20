using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour {

    public Button Exit;


    // Use this for initialization
    void Start () {
        

    }

    void OnMouseDown()
    {

        Exit.onClick.AddListener(() => {
            Application.Quit();
            Debug.Log("Exit was pressed !");
        });
       // Application.Quit();
    }

        // Update is called once per frame
        void Update () {
	
	}
}
