using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Steaming : MonoBehaviour {

    public ParticleSystem Steam;

    public bool flag = true;


    void Start () {
	
	}


    void OnMouseDown()
    {


        // display1.text = starttext1;
        //  display2.text = starttext2;

        //display1.text = "Hello";
        //      display1 = GetComponent<Text>();


        // WORKS !!!!
        //Application.LoadLevel("menu");

        if (flag)
        {
            Steam.Play();
            flag = false;
        }
        else
        {
            Steam.Stop();
            flag = true;
        }
    }

        // Update is called once per frame
        void Update () {
	
	}
}
