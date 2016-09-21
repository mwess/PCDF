using UnityEngine;
using System.Collections;

public class ButtonPush : MonoBehaviour {

    Color def;

	// Use this for initialization
	void Start () {
        def = gameObject.GetComponent<Renderer>().material.color;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("COLLISIION!!!!!!!!!!!!!!!!!!!");
        //gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, 40000f);
        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, 40000000000f);
        //gameObject.GetComponent<Renderer>().material.color = Color.green;

    }

    void OnTriggerExit(Collider other)
    {
        
        //Time.sleep(5);
        gameObject.GetComponent<Renderer>().material.color = def;
    }

    void OnCollisionEnter(Collision other)
    {
        print("Blubb");
    }

}
