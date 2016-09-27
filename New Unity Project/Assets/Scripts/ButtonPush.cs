using UnityEngine;
using System.Collections;


public class ButtonPush : MonoBehaviour {

    Color def;
    float duration = 5;
    float t = 0;

	// Use this for initialization
	void Start () {
        def = gameObject.GetComponent<Renderer>().material.color;

    }

    // Update is called once per frame
    void Update()
    {
        if (t < 1)
        { // while t below the end limit...
          // increment it at the desired rate every update:
            t += Time.deltaTime / duration;
        }
    }

    
    void OnTriggerEnter(Collider other)
    {
        //print("COLLISIION!!!!!!!!!!!!!!!!!!!");
        //gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, 40000f);
        //print("Material color: " + gameObject.GetComponent<Renderer>().material.color);
        //print("Green: " + Color.green);
        //print("Attached rigid body: " + other.attachedRigidbody);
        if (other.attachedRigidbody.ToString() == "model_hand_left (UnityEngine.Rigidbody)" || other.attachedRigidbody.ToString() == "model_hand_right (UnityEngine.Rigidbody)")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

    }
    void OnTriggerStay(Collider other)
    {
        //print("COLLISIION!!!!!!!!!!!!!!!!!!!");
        //gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, 40000f);
        if (other.attachedRigidbody.ToString() == "model_hand_left" || other.attachedRigidbody.ToString() == "model_hand_right")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        //gameObject.GetComponent<Renderer>().material.color = Color.green;

    }

    void OnTriggerExit(Collider other)
    {
        t = 0;
        //GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, t);
        //Time.sleep(5);
        gameObject.GetComponent<Renderer>().material.color = def;
    }

}
