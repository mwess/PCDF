using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {
    public Button ButtonPlay;
    public Button Exit;


    //  public Canvas quitmenu;
    public Button yes;
    public Button no;

    // Use this for initialization
    void Start () {
        Exit.onClick.AddListener(() => {
            Application.Quit();
            Debug.Log("Exit was pressed !");
        });

    }



    public void Buttonplay()
    {
        //  quitmenu.enabled = true;
        yes.enabled = false;
        no.enabled = false;
    }


    public void Exitpress()
    {
        //quitmenu.enabled = false;
        yes.enabled = true;
        no.enabled = true;
    }

    public void OnClick()
    { //Application.LoadLevel("main");
    SceneManager.LoadScene("main");
    }

      //  void OnMouseDown() {  Application.LoadLevel("main");        }

        // Update is called once per frame
        void Update () {
	
	}
}
