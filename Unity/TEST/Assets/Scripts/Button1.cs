using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Button1 : MonoBehaviour {

    public AudioSource sound;

    public Text display1;
    public Text display2;
    public string starttext1 = "Milk        ";
    public string starttext2 = "Extra   70ml";


    public ParticleSystem SingleParticle;

    Text ButtonText;

    public bool flag = true;
  



    void Start () {
        //ButtonText = transform.FindChild("Text").GetComponent<Text>();
        //ButtonText.text = "Exit";


      
    }



  

            void OnMouseDown()
    {


        display1.text = starttext1;
        display2.text = starttext2;

        //display1.text = "Hello";
        //      display1 = GetComponent<Text>();


        // WORKS !!!!
        //Application.LoadLevel("menu");

        if (flag)
        {
            SingleParticle.Play();
            flag = false;
        }
        else
        {
            SingleParticle.Stop();
         flag = true;
        }


    }


    // Update is called once per frame
    void Update () {
        
    }
}
