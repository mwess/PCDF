using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Windows.Kinect;
using System.Linq;
using Leap;
//using "C:\Users\Lu\Documents\PCDF\New Unity Project\LeapSDK\include\Leap.h";


public class DetectJoins2 : MonoBehaviour
{

    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager BodyManager;
    private Body[] bodies;
    public float scalingFactor = 375f;
    Mesh mesh;
    public int numberOfButtons = 9;
    public int numberOfHandPositions = 4;
    public Dictionary<string, int> buttonDict = new Dictionary<string, int>();
    public Button[] buttons;
    public Vector3[] handPositions;
    public Vector3[] handMedPositions;
    public double nearThreshold = 30.0;
    public double collisionThreshold = 10.0;
    public int numberOfFrames = 7;
    public float[,,] handPositionsTimeSteps;
    public int currentFrame = 0;
    public int nDims = 3;
    public float[,] medMatrix;
    public List<float> medList;
    public Color[] colors;
    public Controller leap_controller = new Controller();
    public Vector oldLeap;
    public Vector3 oldKinect;

    public struct Button
    {
        public double distance;
        public Color originalColor;
        public GameObject button;
    }


    // Use this for initialization
    void Start()
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

        buttons = new Button[numberOfButtons];
        handPositions = new Vector3[numberOfHandPositions];
        handPositionsTimeSteps = new float[numberOfFrames, numberOfHandPositions, nDims];
        handMedPositions = new Vector3[numberOfHandPositions];
        medMatrix = new float[numberOfHandPositions, nDims];
        colors = new[] { Color.yellow, Color.yellow, new Color(255, 165, 0, 1), new Color(255, 165, 0, 1), Color.red, Color.red };
        foreach (KeyValuePair<string, int> pair in buttonDict)
        {
            GameObject tmp = GameObject.Find(pair.Key);
            buttons[pair.Value].button = tmp;
            buttons[pair.Value].distance = double.MaxValue;
            buttons[pair.Value].originalColor = tmp.GetComponent<Renderer>().material.color;
            //print(pair.Value + "   " + buttons[pair.Value]);
        }
        if (BodySrcManager == null)
        {
            Debug.Log("Assign BodySourceManager");
        }
        else
        {
            BodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
        //Wissen nicht warum das hier steht, ins Update verschoben
        //GameObject HandLeft = GameObject.Find("model_hand_left");
        //mesh = HandLeft.GetComponent<MeshFilter>().mesh;

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
                GameObject currentHand = GameObject.Find("model_hand_left");
                mesh = currentHand.GetComponent<MeshFilter>().mesh;
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
                    /*
                    Frame frame = leap_controller.Frame();
                    if(frame.Hands.Count > 0)
                    {

                    }
                    else
                    {

                    }
                    */
                    var tmppos = body.Joints[JointType.HandLeft].Position;
                    /*handPositions[0] = new Vector3(scalingFactor* tmppos.X, scalingFactor* tmppos.Y, scalingFactor* tmppos.Z);
                    tmppos = body.Joints[JointType.WristLeft].Position;
                    handPositions[1] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.HandTipLeft].Position;
                    handPositions[2] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    tmppos = body.Joints[JointType.ThumbLeft].Position;
                    handPositions[3] = new Vector3(scalingFactor * tmppos.X, scalingFactor * tmppos.Y, scalingFactor * tmppos.Z);
                    */
                    handPositionsTimeSteps[currentFrame, 0, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 0, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 0, 2] = scalingFactor * tmppos.Z;
                    tmppos = body.Joints[JointType.WristLeft].Position;
                    handPositionsTimeSteps[currentFrame, 1, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 1, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 1, 2] = scalingFactor * tmppos.Z;
                    tmppos = body.Joints[JointType.HandTipLeft].Position;
                    handPositionsTimeSteps[currentFrame, 2, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 2, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 2, 2] = scalingFactor * tmppos.Z;
                    tmppos = body.Joints[JointType.ThumbLeft].Position;
                    handPositionsTimeSteps[currentFrame, 3, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 3, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 3, 2] = scalingFactor * tmppos.Z;
                    currentFrame++;


                    if ((currentFrame + 1) % numberOfFrames != 0)
                    {
                        return;
                    }
                    for (int i = 0; i < numberOfHandPositions; i++)
                    {
                        for (int j = 0; j < nDims; j++)
                        {
                            medMatrix[i, j] = Median(handPositionsTimeSteps, i, j);
                        }
                    }
                    for (int i = 0; i < numberOfHandPositions; i++)
                    {
                        //Subtract z coordinate
                        Vector3 tmp = new Vector3(medMatrix[i, 0], medMatrix[i, 1], -medMatrix[i, 2]);
                        handMedPositions[i] = tmp;
                    }

                    double fac_med_x = 0.0, fac_med_y = 0.0, fac_med_z = 0.0;



                    //print(leap_controller.IsConnected);
                    if (leap_controller.IsConnected)
                    { //controller is a Controller object
                        Frame frame = leap_controller.Frame(); //The latest frame
                        Vector currentLeap = frame.Hands[0].PalmPosition;
                        if (fac_med_x == 0.0)
                        {
                            fac_med_x = currentLeap.x / handMedPositions[0].x;
                        }
                        if (fac_med_y == 0.0)
                        {
                            fac_med_y = currentLeap.y / handMedPositions[0].y;
                        }
                        if (fac_med_z == 0.0)
                        {
                            fac_med_z = currentLeap.z / handMedPositions[0].z;
                        }
                        fac_med_x = (fac_med_x + currentLeap.x / handMedPositions[0].x) / 2;
                        fac_med_y = (fac_med_y + currentLeap.y / handMedPositions[0].y) / 2;
                        fac_med_z = (fac_med_z + currentLeap.z / handMedPositions[0].z) / 2;
                        print(" Current X factor: " + fac_med_x);
                        print(" Current Y factor: " + fac_med_y);
                        print(" Current Z factor: " + fac_med_z);
                        /*print(currentLeap.x);
                        print("Leap: " + frame.Hands[0].PalmPosition);
                        for (int i = 0; i < 1; i++)
                        {
                            print("Hand: " + i + " " + handMedPositions[i]);
                        }*/
                    }

                    currentFrame = 0;
                    Vector3 handtip = new Vector3((medMatrix[2, 0] - medMatrix[0, 0]), (medMatrix[2, 1] - medMatrix[0, 1]), (medMatrix[2, 2] - medMatrix[0, 2]));
                    Vector3 handthumb = new Vector3((medMatrix[3, 0] - medMatrix[0, 0]), (medMatrix[3, 1] - medMatrix[0, 1]), -(medMatrix[3, 2] - medMatrix[0, 2]));
                    Vector3 cross = Vector3.Cross(handthumb, handtip) * 20;
                    handthumb = Vector3.Cross(handtip, cross) * 20;
                    /*foreach (KeyValuePair<string, int> pair in buttonDict)
                    {
                        GameObject tmp = GameObject.Find(pair.Key);
                        buttons[pair.Value].button = tmp;
                        buttons[pair.Value].distance = -1;
                        buttons[pair.Value].originalColor = tmp.GetComponent<Renderer>().material.color;
                        //print(pair.Value + "   " + buttons[pair.Value]);
                    }*/
                    //computeDistanceAndColorButtons(handPositions, buttons);
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
                    /*
                    var pos_jointHandLeft = body.Joints[JointType.HandLeft].Position;

                    var pos_jointWristLeft = body.Joints[JointType.WristLeft].Position;

                    var pos_jointTipLeft = body.Joints[JointType.HandTipLeft].Position;

                    var pos_jointThumbLeft = body.Joints[JointType.ThumbLeft].Position;
                    
                    //gameObject.transform.rotation.Set
                    //Create a Quaternion for the rotation of the hand. Therefore take the vector from the tip to the hand as the forward 
                    //vector for the orientation and from thumb to tip
                    Vector3 handtip = new Vector3((pos_jointTipLeft.X - pos_jointHandLeft.X), (pos_jointTipLeft.Y - pos_jointHandLeft.Y), (pos_jointTipLeft.Z - pos_jointHandLeft.Z));
                    Vector3 handthumb = new Vector3((pos_jointThumbLeft.X - pos_jointHandLeft.X), (pos_jointThumbLeft.Y - pos_jointHandLeft.Y), -(pos_jointThumbLeft.Z - pos_jointHandLeft.Z));
                    Vector3 cross = Vector3.Cross(handthumb, handtip)*20;
                    handthumb = Vector3.Cross(handtip, cross) * 20;
                    


                    //Debug.DrawLine(handPositions[0] - (handtip * 15), handPositions[0] + (handtip* 15 ), Color.blue);
                    //Debug.DrawLine(handPositions[0] - (handthumb * 15), handPositions[0] + (handthumb * 15), Color.red);
                    //Debug.DrawLine(handPositions[0] - (cross * 15), handPositions[0] + (cross * 15), Color.green);
                    */
                    Vector3 forward = handthumb;
                    Vector3 up = Vector3.Cross(handtip, handthumb);
                    //forward = -up;

                    Quaternion LeftHandRotation = Quaternion.LookRotation(forward, up);
                    
                    LeftHandRotation.x = LeftHandRotation.x;
                    LeftHandRotation.y = LeftHandRotation.y;
                    LeftHandRotation.z = -LeftHandRotation.z;
                    LeftHandRotation.w = -LeftHandRotation.w;

                    gameObject.transform.rotation = LeftHandRotation;
                    gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
                    gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, -pos.Z * scalingFactor);

                    //print("Position: " + pos.X* scalingFactor + " " + pos.Y * scalingFactor + " " + (pos.Z*scalingFactor-350));
                    //Make hands transparent
                    Color temp = new Color(gameObject.GetComponent<Renderer>().material.color.r, gameObject.GetComponent<Renderer>().material.color.g, gameObject.GetComponent<Renderer>().material.color.b, 0.5f);
                    gameObject.GetComponent<Renderer>().material.color = temp;

                    var tmpHandPos = GameObject.Find("model_hand_right").transform.position;
                    var button1Pos = GameObject.Find("Cylinder_036").transform.position;
                    if (euclideanDistance(tmpHandPos, button1Pos) > euclideanDistance(tmpHandPos, gameObject.transform.position))
                    {
                        computeDistanceAndColorButtons(handMedPositions);
                    }
                    //for(int i =0; i < handMedPositions.Length; i++)
                    //{
                    ///     print("Position: " + handMedPositions[i].x + " " + handMedPositions[i].y + " " + (handMedPositions[i].z));
                    // }



                }
                else if (TrackedJoint.ToString() == "HandRight")
                {
                    /*
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

                    */


                    //print("Position Right: ");
                    //foreach (var b in handPositions)
                    //{
                    //    print(b);
                    //}
                    /*
                    print("HandRight: " + gameObject.transform.position);
                    print(handPositions[0]);
                    print(handPositions[1]);
                    print(handPositions[2]);
                    print(handPositions[3]);*/
                    /*
                    Vector3 handtip = new Vector3(pos_jointTipRight.X - pos_jointHandRight.X, pos_jointTipRight.Y - pos_jointHandRight.Y, pos_jointTipRight.Z - pos_jointHandRight.Z);
                    Vector3 handthumb = new Vector3(pos_jointThumbRight.X - pos_jointHandRight.X, pos_jointThumbRight.Y - pos_jointHandRight.Y, pos_jointThumbRight.Z - pos_jointHandRight.Z);
                    Vector3 cross = Vector3.Cross(handthumb, handtip) * 20;
                    handthumb = Vector3.Cross(handtip, cross) * 20;
                    */
                    //Debug.DrawLine(handPositions[0] - (handtip * 15), handPositions[0] + (handtip * 15), Color.blue);
                    //Debug.DrawLine(handPositions[0] - (handthumb * 15), handPositions[0] + (handthumb * 15), Color.red);
                    //Debug.DrawLine(handPositions[0] - (cross * 15), handPositions[0] + (cross * 15), Color.green);
                    /*
                    Vector3 forward = handthumb;
                    Vector3 up = Vector3.Cross(handtip, handthumb);

                    Quaternion RightHandRotation = Quaternion.LookRotation(forward, up);
                    RightHandRotation.x = RightHandRotation.x;
                    RightHandRotation.y = RightHandRotation.y;
                    RightHandRotation.z = -RightHandRotation.z;
                    RightHandRotation.w = -RightHandRotation.w;
                    */
                    //foreach (KeyValuePair<string, int> pair in buttonDict)
                    //{
                    //    GameObject tmp = GameObject.Find(pair.Key);
                    //    buttons[pair.Value].button = tmp;
                    //    buttons[pair.Value].distance = -1;
                    //    buttons[pair.Value].originalColor = tmp.GetComponent<Renderer>().material.color;
                    //print(pair.Value + "   " + buttons[pair.Value]);
                    //}
                    /*
                    computeDistanceAndColorButtons(handPositions);
                 
                    gameObject.transform.rotation = RightHandRotation;
                    gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, -pos.Z * scalingFactor);
                    */
                    Color temp = new Color(gameObject.GetComponent<Renderer>().material.color.r, gameObject.GetComponent<Renderer>().material.color.g, gameObject.GetComponent<Renderer>().material.color.b, 0.5f);
                    gameObject.GetComponent<Renderer>().material.color = temp;
                    


                    //gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, pos.Z * scalingFactor);


                    //Object.FindObjectOfType(typeof(MonoBehaviour));
                    var tmppos = body.Joints[JointType.HandRight].Position;
                    handPositionsTimeSteps[currentFrame, 0, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 0, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 0, 2] = scalingFactor * tmppos.Z;
                    tmppos = body.Joints[JointType.WristRight].Position;
                    handPositionsTimeSteps[currentFrame, 1, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 1, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 1, 2] = scalingFactor * tmppos.Z;
                    tmppos = body.Joints[JointType.HandTipRight].Position;
                    handPositionsTimeSteps[currentFrame, 2, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 2, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 2, 2] = scalingFactor * tmppos.Z;
                    tmppos = body.Joints[JointType.ThumbRight].Position;
                    handPositionsTimeSteps[currentFrame, 3, 0] = scalingFactor * tmppos.X;
                    handPositionsTimeSteps[currentFrame, 3, 1] = scalingFactor * tmppos.Y;
                    handPositionsTimeSteps[currentFrame, 3, 2] = scalingFactor * tmppos.Z;
                    currentFrame++;


                    if ((currentFrame + 1) % numberOfFrames != 0)
                    {
                        return;
                    }
                    for (int i = 0; i < numberOfHandPositions; i++)
                    {
                        for (int j = 0; j < nDims; j++)
                        {
                            medMatrix[i, j] = Median(handPositionsTimeSteps, i, j);
                        }
                    }
                    for (int i = 0; i < numberOfHandPositions; i++)
                    {
                        //Subtract from z coordinate
                        Vector3 tmp = new Vector3(medMatrix[i, 0], medMatrix[i, 1], -medMatrix[i, 2]);
                        handMedPositions[i] = tmp;
                    }
                    currentFrame = 0;
                    Vector3 handtip2 = new Vector3((medMatrix[2, 0] - medMatrix[0, 0]), (medMatrix[2, 1] - medMatrix[0, 1]), (medMatrix[2, 2] - medMatrix[0, 2]));
                    Vector3 handthumb2 = new Vector3((medMatrix[3, 0] - medMatrix[0, 0]), (medMatrix[3, 1] - medMatrix[0, 1]), -(medMatrix[3, 2] - medMatrix[0, 2]));
                    Vector3 cross2 = Vector3.Cross(handthumb2, handtip2) * 20;
                    handthumb2 = Vector3.Cross(handtip2, cross2) * 20;

                    //computeDistanceAndColorButtons(handMedPositions);

                    Vector3 forward2 = handthumb2;
                    Vector3 up2 = Vector3.Cross(handthumb2, handtip2);
                    //forward = -up;

                    Quaternion RightHandRotation = Quaternion.LookRotation(forward2, up2);
                    RightHandRotation.x = RightHandRotation.x;
                    RightHandRotation.y = RightHandRotation.y;
                    RightHandRotation.z = -RightHandRotation.z;
                    RightHandRotation.w = -RightHandRotation.w;
                    gameObject.transform.rotation = RightHandRotation;
                    gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
                    gameObject.transform.position = new Vector3(pos.X * scalingFactor, pos.Y * scalingFactor, -pos.Z * scalingFactor);

                    //Make hands transparent
                    //Color temp2 = new Color(gameObject.GetComponent<Renderer>().material.color.r, gameObject.GetComponent<Renderer>().material.color.g, gameObject.GetComponent<Renderer>().material.color.b, 0.5f);
                    //gameObject.GetComponent<Renderer>().material.color = temp2;

                    var tmpHandPos = GameObject.Find("model_hand_left").transform.position;
                    var button1Pos = GameObject.Find("Cylinder_036").transform.position;
                    if (euclideanDistance(tmpHandPos, button1Pos) > euclideanDistance(tmpHandPos, gameObject.transform.position))
                    {
                        computeDistanceAndColorButtons(handMedPositions);
                    }
                }

            }

        }
    }
    void computeDistanceAndColorButtons(Vector3[] hand)
    {
        double dist = 0;
        for (int i = 0; i < hand.Length; i++)
        {
            for (int j = 0; j < buttons.Length; j++)
            {
                dist = euclideanDistance(hand[i], buttons[j].button.transform.position);
                /*if(dist < 20)
                {
                    print("Dist: " + dist);
                }*/
                if (j == 0)
                {
                    buttons[j].distance = dist;
                }
                else if (dist < buttons[j].distance)
                {
                    buttons[j].distance = dist;
                }
            }
        }
        for (int i = 0; i < hand.Length; i++)
        {
            //print("Hand: " + i + " " + hand[i]);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            //print(i + " " + buttons[i].distance + " " + buttons[i].button.transform.position);
        }
        Array.Sort<Button>(buttons, (x, y) => x.distance.CompareTo(y.distance));
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < colors.Length && buttons[i].distance < nearThreshold)
            {
                if (buttons[i].button.GetComponent<Renderer>().material.color != Color.green)
                {
                    buttons[i].button.GetComponent<Renderer>().material.color = colors[i];
                }
                //print("Set Color to " + colors[i]);
            }
            else if (buttons[i].distance >= nearThreshold)
            {
                if (buttons[i].button.GetComponent<Renderer>().material.color != Color.green)
                {
                    buttons[i].button.GetComponent<Renderer>().material.color = buttons[i].originalColor;
                }
                //print("Reset color: " + buttons[i].button);
            }
        }
    }

    double euclideanDistance(Vector3 x, Vector3 y)
    {
        double dist = 0;
        //print("X: " + x.x + " Y: " + x.y + " Z: " + x.z);
        dist += Math.Pow(x.x - y.x, 2);
        dist += Math.Pow(x.y - y.y, 2);
        dist += Math.Pow(x.z - y.z, 2);
        return Math.Sqrt(dist);
    }



    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
    }


    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(150f, 200f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
    private int Partition(int start, int end, System.Random rnd = null)
    {
        if (rnd != null)
            Swap(end, rnd.Next(start, end));

        var pivot = medList[end];
        var lastLow = start - 1;
        for (var i = start; i < end; i++)
        {
            if (medList[i].CompareTo(pivot) <= 0)
                Swap(i, ++lastLow);
        }
        Swap(end, ++lastLow);
        return lastLow;
    }

    /// <summary>
    /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
    /// Note: specified list would be mutated in the process.
    /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
    /// </summary>
    public float NthOrderStatistic(int n, System.Random rnd = null)
    {
        return NthOrderStatistic(n, 0, medList.Count - 1, rnd);
    }
    private float NthOrderStatistic(int n, int start, int end, System.Random rnd)
    {
        while (true)
        {
            var pivotIndex = Partition(start, end, rnd);
            if (pivotIndex == n)
                return medList[pivotIndex];

            if (n < pivotIndex)
                end = pivotIndex - 1;
            else
                start = pivotIndex + 1;
        }
    }

    public void Swap(int i, int j)
    {
        if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
            return;
        var temp = medList[i];
        medList[i] = medList[j];
        medList[j] = temp;
    }

    /// <summary>
    /// Note: specified list would be mutated in the process.
    /// </summary>
    public float Median(float[,,] cube, int f, int s)
    {
        //var list = sequence.Select(getValue).ToList();
        convertArrayToList(cube, f, s);
        var mid = (medList.Count - 1) / 2;
        return NthOrderStatistic(mid);
    }

    public void convertArrayToList(float[,,] cube, int f, int s)
    {
        medList = new List<float>();
        for (int i = 0; i < numberOfFrames; i++)
        {
            medList.Add(cube[i, f, s]);
        }
    }
}
