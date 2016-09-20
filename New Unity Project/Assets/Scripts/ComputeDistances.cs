using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ComputeDistances : MonoBehaviour {

    public GameObject HandRight;
    public GameObject HandLeft;
	public GameObject[] buttons; 
	public Dictionary<string, int> buttons = new Dictionary<string,int>();
    public double DistanceLeft;
    public double DistanceRight;
    public double Threshold = 10d;

	// Use this for initialization
	void Start () {
		buttons.Add ("Bigbuttondown", 1);
		buttons.Add ("Bigbuttonleft", 2);
		buttons.Add ("Bigbuttonright", 3);
		buttons.Add ("Bigbuttonup", 4);
		buttons.Add ("TOUCH1_1", 5);
		buttons.Add ("TOUCH2_2", 6);
		buttons.Add ("TOUCH3_3", 7);
		buttons.Add ("TOUCH4_4", 8);
		buttons.Add ("TOUCH5_5", 9);
		buttons.Add ("ExitButton", 10);
	
	}

    // Update is called once per frame
    void Update() {
        HandRight = GameObject.Find("model_hand_right");
        HandLeft = GameObject.Find("model_hand_left");


        

		// Update left hand
        DistanceLeft = euclideanDistance(HandLeft.transform.position, gameObject.transform.position);
        // Update right hand
        DistanceRight = euclideanDistance(HandRight.transform.position, gameObject.transform.position);
        //print("rEFRESH?");
        //print("check the new positions");
        //print("Position of Left Hand: " + HandLeft.transform.position);
        //print("Position of Right Hand: " + HandRight.transform.position);
        //print("Distance left:" + DistanceLeft);
        

        // Chekc traffic light


        if (DistanceLeft <= Threshold && DistanceRight <= Threshold)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        } else if (DistanceLeft <= Threshold)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        } else if (DistanceRight <= Threshold)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        } else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        
	}

    double euclideanDistance(Vector3 x, Vector3 y)
    {
        double dist = 0;
        dist += Math.Pow(x.x - y.x, 2);
        dist += Math.Pow(x.y - y.y, 2);
        dist += Math.Pow(x.z - y.z, 2);
        return Math.Sqrt(dist);
    }
}
