using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Windows.Kinect;

public class DetectJoins : MonoBehaviour {

    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager BodyManager;
    private Body[] bodies;
    public float multiplier = 10f;
    Mesh mesh;
	public int numberOfButtons = 10;
	public int numberOfHandPositions = 4;
	public Dictionary<string, int> buttonDict = new Dictionary<string,int>();
	public GameObject[] buttons = new GameObject[numberOfButtons];
	public Vector3[] handPositions = new Vector3[numberOfHandPositions]; 
	public double nearThreshold = 5d;
	public double collisionThreshold = 1d;


    // Use this for initialization
    void Start ()
    {
		buttonDict.Add ("Bigbuttondown", 1);
		buttonDict.Add ("Bigbuttonleft", 2);
		buttonDict.Add ("Bigbuttonright", 3);
		buttonDict.Add ("Bigbuttonup", 4);
		buttonDict.Add ("TOUCH1_1", 5);
		buttonDict.Add ("TOUCH2_2", 6);
		buttonDict.Add ("TOUCH3_3", 7);
		buttonDict.Add ("TOUCH4_4", 8);
		buttonDict.Add ("TOUCH5_5", 9);
		buttonDict.Add ("ExitButton", 10);

        if (BodySrcManager == null)
        {
            Debug.Log("Assign BodySourceManager");
        }
        else
        {
            BodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
        GameObject HandLeft = GameObject.Find("model_hand_left");
        mesh = HandLeft.GetComponent<MeshFilter>().mesh;

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(BodyManager == null)
        {
            return;
        }
        bodies = BodyManager.GetData();
        
        if(bodies == null)
        {
            return;
        }
        foreach(var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if(body.IsTracked)
            {

                var pos = body.Joints[TrackedJoint].Position;

                var rot = gameObject.transform.eulerAngles;
                /*Vector3[] verts;
                Vector3 vertPos;
                GameObject[] handles;
                mesh = GetComponent<MeshFilter>().mesh;
                verts = mesh.vertices;
                print("I got " + verts.Length + " vertices-sets");
                /*foreach (Vector3 vert in verts)
                {
                    print("Vert is: (x: " + vert[0] + "," + vert[1] + "," + vert[2] + ")");
                }*/

                /*Vector3[] vertices = mesh.vertices;
                print("------------------");
                print("Got vertices:" + vertices);
                int i = 0;
                while (i < vertices.Length)
                {
                    print("vertices[" + i + "]: " + vertices[i]);
                    i++;
                }*/
                print("-------");
                print("Logging joint " + TrackedJoint + "towards (" + pos.X * multiplier + "," + pos.Y * multiplier + "," + pos.Z * multiplier + ")" );
                print("Rotation is: (x: " + rot.x + ", y:" + rot.y + ", z: " + rot.z + ")");
                if ( TrackedJoint.ToString() == "HandLeft")
                {
					handPositions[0] = body.Joints[JointType.HandLeft].Position;
					handPositions[1] = body.Joints[JointType.WristLeft].Position;
					handPositions[2] = body.Joints[JointType.HandTipLeft].Position;
					handPositions[3] = body.Joints[JointType.ThumbLeft].Position;

					var pos_jointHandLeft = body.Joints[JointType.HandLeft].Position;
					var pos_jointWristLeft = body.Joints[JointType.WristLeft].Position;
					var pos_jointTipLeft = body.Joints[JointType.HandTipLeft].Position;
					var pos_jointThumbLeft = body.Joints[JointType.ThumbLeft].Position;                    
				} else if ( TrackedJoint.ToString() == "HandRight")
                {
					handPositions[0] = body.Joints[JointType.HandRight].Position;
					handPositions[1] = body.Joints[JointType.WristRight].Position;
					handPositions[2] = body.Joints[JointType.HandTipRight].Position;
					handPositions[3] = body.Joints[JointType.ThumbRight].Position;

                    var pos_jointHandRight = body.Joints[JointType.HandRight].Position;
                    var pos_jointWristRight = body.Joints[JointType.WristRight].Position;
                    var pos_jointTipRight = body.Joints[JointType.HandTipRight].Position;
                    var pos_jointThumbRight = body.Joints[JointType.ThumbRight].Position;

                }
				foreach (KeyValuePair<string,int> pair in buttonDict) 
				{
					buttons [pair.Value] = GameObject.Find (pair.Key);
				}
				int minKey = computeDistanceAndColorButtons (handPositions, buttons);
                gameObject.transform.position = new Vector3(pos.X * multiplier, pos.Y * multiplier, pos.Z * multiplier);
                //Object.FindObjectOfType(typeof(MonoBehaviour));
            }
        }
	}
	void computeDistanceAndColorButtons (Vector3[] hand, GameObject[] buttons )
	{
		double dist = 0, minDist = 999999999; 
		int minIndex = -1;
		for(int i = 0; i < hand.Length(); i++)
		{
			for(int j = 0; j < buttons.Length(); j++)
			{
				dist = euclideanDistance(handPositions[i], buttons[j].transform.position);
				if(dist <= nearThreshold)
				{
					//color object, if a certain threshold is crossed
					buttons[j].GetComponent<Renderer>().material.color = Color.yellow;
				}
				else
				{
					buttons[j].GetComponent<Renderer>().material.color = Color.white;
				}
				if(dist < minDist)
				{
					minDist = dist;
					minIndex = j;
				}

			}
		}
		if(minDist <= collisionThreshold)
		{
			buttons[minIndex].GetComponent<Renderer>().material.color = Color.green;
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
