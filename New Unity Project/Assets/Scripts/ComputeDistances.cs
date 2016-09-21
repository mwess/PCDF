using UnityEngine;
using System.Collections;
using System;

public class ComputeDistances : MonoBehaviour {

    public GameObject HandRight;
    public GameObject HandLeft;
    public double DistanceLeft;
    public double DistanceRight;
    public double Threshold = 10d;

	// Use this for initialization
	void Start () {
	
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
        /*
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
        */
        
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
