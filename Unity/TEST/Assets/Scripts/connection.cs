using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class connection : MonoBehaviour {


    //  public Text UIText1 ;
    //private Text UIText2 = null;

    public AudioSource sound;


    public Text display1;
    public Text display2;

   // public string starttext1 = "   --||--   ";
    //public string starttext2 = "Gerät Bereit";

    public string starttext1 = null;
    public string starttext2 = null;

    public string endtext = "Coffee";
   
    public bool processing = true;
    public string emptyscreen = " ";

    public ParticleSystem particles;
    public ParticleSystem particles1;

    public ParticleSystem SingleParticle;

    public bool doubleemitterflag = true;
    public bool singleemitterflag = true;

    public bool flag = true;



    void Start () {

        
        // display1 = GetComponent<Text>();
        // display2 = GetComponent<Text>();

        StartCoroutine(StartBlinking());

        StartCoroutine(StopBlinking());

    }


    

    public IEnumerator StartBlinking()
    {
            while (processing)      {

            display1.text = emptyscreen;
            display2.text = emptyscreen;
            yield return new WaitForSeconds(.85f);


            display1.text = starttext1;
            display2.text = starttext2;
            yield return new WaitForSeconds(0.85f);
       }
    }
  
    IEnumerator StopBlinking()
    {
       
        yield return new WaitForSeconds(8000f);
      
        processing = false;

     //   display1.text = endtext;
     //   display2.text = "Press Exit";
    }



    void OnMouseDown()
    {

        //display1.text = "Hello";




        //   display1 = gameObject.GetComponent<Text>();
        // display2 = gameObject.GetComponent<Text>();

      


        // WORKS !!!!
        //Application.LoadLevel("menu");

        if (flag)
        {
            particles.Play();
            particles1.Play();
           flag = false;
        }
        else
        {
            particles.Stop();
            particles1.Stop();
           flag = true;
        }




/*

        if (singleemitterflag)
        {
            particles.Play();
            particles1.Play();
            singleemitterflag = false;
        }
        else
        {
            particles.Stop();
            particles1.Stop();
            singleemitterflag = true;
        }
        */

    }
    
    
    
    // Update is called once per frame
        void Update () {
     
        // if (Input.GetKeyDown(KeyCode))


    }
}
