using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Button5 : MonoBehaviour {

    public ParticleSystem particles;
    public ParticleSystem particles1;


    public Text displayup = null;
    public Text displaydown = null;

    public string starttext1 = "1 KAFFEE";
    public string starttext2 = "|    | 210ml";

    public bool processing = true;
    public string emptyscreen =null;
    public bool flag = true;

    void Start () {
        StartCoroutine(StartCoffee());

        StartCoroutine(StopCoffee());

    }


    public IEnumerator StartCoffee()
    {
        while (processing)
        {

            displayup.text = emptyscreen;
            displaydown.text = emptyscreen;
            yield return new WaitForSeconds(.85f);

            // displayup =GetComponent<Text>();
            // displaydown = transform.FindChild("Text").GetComponent<Text>();
            displayup.text = starttext1;
            displaydown.text = starttext2;
            Debug.Log(displayup);
            Debug.Log(displaydown);
            
            yield return new WaitForSeconds(.85f);
        }
    }

    IEnumerator StopCoffee()
    {

        yield return new WaitForSeconds(8f);

        processing = false;

        //   display1.text = endtext;
        //   display2.text = "Press Exit";
    }


    void OnMouseDown()
    {
                
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

    }



    void Update () {

        }
    }
