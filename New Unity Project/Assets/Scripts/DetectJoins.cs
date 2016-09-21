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
    public float scalingFactor = 375f;
    Mesh mesh;
    public int numberOfButtons = 9;
    public int numberOfHandPositions = 4;
    public Dictionary<string, int> buttonDict = new Dictionary<string, int>();
    public GameObject[] buttons;
    public Vector3[] handPositions;
    public double nearThreshold = 30.0;
    public double collisionThreshold = 10.0;
    //public float tmpmult = 375f;
    public double counter = 0;
    public double sum = 0;


   
    // Use this for initialization
    void Start ()
    {
        buttonDict.Add("Bigbuttondown", 0);
        buttonDict.Add("Bigbuttonleft", 1);
        buttonDict.Add("Bigbuttonright", 2);
        buttonDict.Add("Bigbuttonup", 3);
        buttonDict.Add("TOUCH1_1", 4);
        buttonDict.Add("TOUCH2_2", 5);
        buttonDict.Add("TOUCH3_3", 6);
        buttonDict.Add("TOUCH4_4", 7);
        buttonDict.Add("TOUCH5_5", 8);
        //buttonDict.Add("ExitButton", 9);

        buttons = new GameObject[numberOfButtons];
        handPositions = new Vector3[numberOfHandPositions];

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
    void Update()
    {
        if (BodyManager == null)
        {
            return;
        }
        bodies = BodyManager.GetData();

        if (bodies == null)
        {
            return;
        }
        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {

                var pos = body.Joints[TrackedJoint].Position;

                //var rot = gameObject.transform.eulerAngles;
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
                //print("-------");
                //print("Logging joint " + TrackedJoint + "towards (" + pos.X * scalingFactor + "," + pos.Y * scalingFactor + "," + pos.Z * scalingFactor + ")");
                //print("Rotation is: (x: " + rot.x + ", y:" + rot.y + ", z: " + rot.z + ")");
                if (TrackedJoint.ToString() == "HandLeft")
                {
                    var tmppos = body.Joints[JointType.HandLeft].Position;
                    
                    handPositions[0] = new Vector3(scalingFactor* tmppos.X, scalingFactor* tmppos.Y, scalingFactor* tmppos.Z);
                    tmppos = body.Joints[JointType.WristLeft].Position;
                    handPositions[1] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.HandTipLeft].Position;
                    handPositions[2] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.ThumbLeft].Position;
                    handPositions[3] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);

                    //print(gameObject.transform.position.x);
                    //print(handPositions[0].x);
                    /*sum += Math.Abs(gameObject.transform.position.x / handPositions[0].x);
                    counter += 1;
                    print("Data: ");
                    print(Math.Abs(gameObject.transform.position.x / handPositions[0].x));
                    print(counter);
                    print(sum / counter);*/
                    //print(handPositions[1]);
                    //print(handPositions[2]);
                    //print(handPositions[3]);

                    var pos_jointHandLeft = body.Joints[JointType.HandLeft].Position;

                    var pos_jointWristLeft = body.Joints[JointType.WristLeft].Position;

                    var pos_jointTipLeft = body.Joints[JointType.HandTipLeft].Position;

                    var pos_jointThumbLeft = body.Joints[JointType.ThumbLeft].Position;

                    //gameObject.transform.rotation.Set
                    //Create a Quaternion for the rotation of the hand. Therefore take the vector from the tip to the hand as the forward 
                    //vector for the orientation and from thumb to tip
                    Vector3 handtip = new Vector3((pos_jointTipLeft.X - pos_jointHandLeft.X), (pos_jointTipLeft.Y - pos_jointHandLeft.Y), (pos_jointTipLeft.Z - pos_jointHandLeft.Z));
                    Vector3 handthumb = new Vector3((pos_jointThumbLeft.X - pos_jointHandLeft.X), (pos_jointThumbLeft.Y - pos_jointHandLeft.Y), -(pos_jointThumbLeft.Z - pos_jointHandLeft.Z));




                    Vector3 forward = handthumb;
                    Vector3 up = Vector3.Cross(handthumb, handtip);
                    //forward = -up;

                    Quaternion LeftHandRotation = Quaternion.LookRotation(forward, up);
                    LeftHandRotation.x = LeftHandRotation.x;
                    LeftHandRotation.y = LeftHandRotation.y;
                    LeftHandRotation.z = LeftHandRotation.z;
                    LeftHandRotation.w = LeftHandRotation.w;

                    gameObject.transform.rotation = LeftHandRotation;
                    gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, pos.Z * scalingFactor);


                }
                else if (TrackedJoint.ToString() == "HandRight")
                {
                    var tmppos = body.Joints[JointType.HandRight].Position;
                    handPositions[0] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.WristRight].Position;
                    handPositions[1] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.HandTipRight].Position;
                    handPositions[2] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.ThumbRight].Position;
                    handPositions[3] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);

                    var pos_jointHandRight = body.Joints[JointType.HandRight].Position;
                    var pos_jointWristRight = body.Joints[JointType.WristRight].Position;
                    var pos_jointTipRight = body.Joints[JointType.HandTipRight].Position;
                    var pos_jointThumbRight = body.Joints[JointType.ThumbRight].Position;

                    /*
                    print("HandRight: " + gameObject.transform.position);
                    print(handPositions[0]);
                    print(handPositions[1]);
                    print(handPositions[2]);
                    print(handPositions[3]);*/
                    Vector3 handtip = new Vector3(pos_jointTipRight.X - pos_jointHandRight.X, pos_jointTipRight.Y - pos_jointHandRight.Y, pos_jointTipRight.Z - pos_jointHandRight.Z);
                    Vector3 handthumb = new Vector3(pos_jointThumbRight.X - pos_jointHandRight.X, pos_jointThumbRight.Y - pos_jointHandRight.Y, pos_jointThumbRight.Z - pos_jointHandRight.Z);

                    Vector3 forward = handthumb;
                    Vector3 up = Vector3.Cross(handtip, handthumb);

                    Quaternion RightHandRotation = Quaternion.LookRotation(forward, up);

                    foreach (KeyValuePair<string, int> pair in buttonDict)
                    {
                        buttons[pair.Value] = GameObject.Find(pair.Key);
                        //print(pair.Value + "   " + buttons[pair.Value]);
                    }
                    computeDistanceAndColorButtons(handPositions, buttons);
                    gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, pos.Z * scalingFactor);
                    gameObject.transform.rotation = RightHandRotation;


                }

                //gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, pos.Z * scalingFactor);


                //Object.FindObjectOfType(typeof(MonoBehaviour));
            }

        }
    }
    void computeDistanceAndColorButtons (Vector3[] hand, GameObject[] buttons)
    {
        double dist = 0, minDist = 999999999;
        int minIndex = -1;
        for (int i = 0; i < hand.Length; i++)
        {
            for (int j = 0; j < buttons.Length; j++)
            {
                dist = euclideanDistance(handPositions[i], buttons[j].transform.position);
                //print("handPositions: " + handPositions[i]);
                //print("buttons: " + buttons[j].transform.position);
                //print("dist: "+ dist);
                if (dist <= nearThreshold)
                {
                    //print("YELLOW "+ i + " " + j);
                    //color object, if a certain threshold is crossed
                    buttons[j].GetComponent<Renderer>().material.color = Color.yellow;

                }
                else
                {
                    buttons[j].GetComponent<Renderer>().material.color = Color.white;
                }
                if (dist < minDist)
                {
                    minDist = dist;
                    minIndex = j;
                }

            }
        }
        //print("minDist: " + minDist);
        if (minDist <= collisionThreshold)
        {
            print("GREEN: " + minDist + " " + minIndex);
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
  


    public static Quaternion QuaternionFromMatrix(Matrix4x4 m) { return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1)); }


}
