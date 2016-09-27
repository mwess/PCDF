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
    public bool trackingLeap;
    public bool isRightHand;

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
        isRightHand = TrackedJoint.ToString() == "RightHand" ? true : false;
        buttons = new Button[numberOfButtons];
        handPositions = new Vector3[numberOfHandPositions];
        handPositionsTimeSteps = new float[numberOfFrames, numberOfHandPositions, nDims];
        handMedPositions = new Vector3[numberOfHandPositions];
        medMatrix = new float[numberOfHandPositions, nDims];
        colors = new[] {Color.red, Color.yellow, Color.yellow, Color.yellow};
        foreach (KeyValuePair<string, int> pair in buttonDict)
        {
            GameObject tmp = GameObject.Find(pair.Key);
            buttons[pair.Value].button = tmp;
            buttons[pair.Value].distance = double.MaxValue;
            buttons[pair.Value].originalColor = tmp.GetComponent<Renderer>().material.color;
        }
        if (BodySrcManager == null)
        {
            Debug.Log("Assign BodySourceManager");
        }
        else
        {
            BodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
        // find out which device is used at start
        Frame frame = leap_controller.Frame();
        trackingLeap = frame.Hands.Count > 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject currentHand = GameObject.Find("model_hand_left");
        GameObject currentHand = gameObject;
        mesh = currentHand.GetComponent<MeshFilter>().mesh;
        Frame frame = leap_controller.Frame();
        makeHandsTransparent();
        //if (frame.Hands.Count > 0)
        if (false)
        {
            print("Leap!!!!!");
            //reset frames when the device changes.
            if (!trackingLeap)
            {
                currentFrame = 0;
                trackingLeap = true;
            }
            HandList hands = frame.Hands;
            foreach (Hand hand in hands)
            {
                if (TrackedJoint.ToString() == "HandLeft" && hand.IsLeft)
                {
                    isRightHand = false;
                }
                else if (TrackedJoint.ToString() == "HandRight" && hand.IsRight)
                {
                    isRightHand = true;
                }
                computeLeapFrameMedian(isRightHand, hand);
            }
        }
        else
        {
            //print("Kinect!!!");
            if (trackingLeap)
            {
                currentFrame = 0;
                trackingLeap = false;
            }
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
                    //GameObject currentHand = GameObject.Find("model_hand_left");
                    //mesh = currentHand.GetComponent<MeshFilter>().mesh;
                    //var pos = body.Joints[TrackedJoint].Position;
                    if (TrackedJoint.ToString() == "HandLeft")
                    {
                        isRightHand = false;
                    }
                    else if (TrackedJoint.ToString() == "HandRight")
                    {
                        isRightHand = true;
                    }
                    computeKinectFrameMedian(isRightHand, body);
                    
                }
            }
        }
    }

    void computeKinectFrameMedian(bool isRightHand, Body body)
    {
        var tmppos = new CameraSpacePoint();
        if (isRightHand)
        {
            tmppos = body.Joints[JointType.HandRight].Position;
        }
        else
        {
            tmppos = body.Joints[JointType.HandLeft].Position;
        }
        handPositionsTimeSteps[currentFrame, 0, 0] = scalingFactor * tmppos.X;
        handPositionsTimeSteps[currentFrame, 0, 1] = scalingFactor * tmppos.Y;
        handPositionsTimeSteps[currentFrame, 0, 2] = scalingFactor * tmppos.Z;
        if (isRightHand)
        {
            tmppos = body.Joints[JointType.WristRight].Position;
        }
        else
        {
            tmppos = body.Joints[JointType.WristLeft].Position;
        }
        handPositionsTimeSteps[currentFrame, 1, 0] = scalingFactor * tmppos.X;
        handPositionsTimeSteps[currentFrame, 1, 1] = scalingFactor * tmppos.Y;
        handPositionsTimeSteps[currentFrame, 1, 2] = scalingFactor * tmppos.Z;
        if (isRightHand)
        {
            tmppos = body.Joints[JointType.HandTipRight].Position;
        }
        else
        {
            tmppos = body.Joints[JointType.HandTipLeft].Position;
        }
        handPositionsTimeSteps[currentFrame, 2, 0] = scalingFactor * tmppos.X;
        handPositionsTimeSteps[currentFrame, 2, 1] = scalingFactor * tmppos.Y;
        handPositionsTimeSteps[currentFrame, 2, 2] = scalingFactor * tmppos.Z;
        if (isRightHand)
        {
            tmppos = body.Joints[JointType.ThumbRight].Position;
        }
        else
        {
            tmppos = body.Joints[JointType.ThumbLeft].Position;
        }
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
        var tmp = body.Joints[TrackedJoint].Position;
        Vector3 tmpvec = new Vector3(tmp.X, tmp.Y, tmp.Z);
        calculatePositions(tmpvec);
        calculateActiveHand();
    }

    void calculatePositions(Vector3 pos) {
        for (int i = 0; i < numberOfHandPositions; i++)
        {
            //Subtract from z coordinate
            Vector3 tmp = new Vector3(medMatrix[i, 0], medMatrix[i, 1], -medMatrix[i, 2]);
            handMedPositions[i] = tmp;
        }
        currentFrame = 0;

        Vector3 handtip = new Vector3((medMatrix[2, 0] - medMatrix[0, 0]), (medMatrix[2, 1] - medMatrix[0, 1]), (medMatrix[2, 2] - medMatrix[0, 2]));
        Vector3 handthumb = new Vector3((medMatrix[3, 0] - medMatrix[0, 0]), (medMatrix[3, 1] - medMatrix[0, 1]), -(medMatrix[3, 2] - medMatrix[0, 2]));
        Vector3 cross = Vector3.Cross(handthumb, handtip) * 20;

        handthumb = Vector3.Cross(handtip, cross) * 20;
        Vector3 forward = handthumb;

        if (isRightHand)
        {
            Vector3 up = Vector3.Cross(handthumb, handtip);

            Quaternion RightHandRotation = Quaternion.LookRotation(forward, up);
            RightHandRotation.x = RightHandRotation.x;
            RightHandRotation.y = RightHandRotation.y;
            RightHandRotation.z = -RightHandRotation.z;
            RightHandRotation.w = -RightHandRotation.w;
            gameObject.transform.rotation = RightHandRotation;
            gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
            print("Pos: " + pos);
            print("handMedPosition: " + handMedPositions[0]);
            //gameObject.transform.position = new Vector3(pos.x * scalingFactor, pos.y * scalingFactor, -pos.z * scalingFactor);
            gameObject.transform.position = new Vector3(handMedPositions[0].x, handMedPositions[0].y, handMedPositions[0].z);
        }
        else
        {
            Vector3 up = Vector3.Cross(handtip, handthumb);

            Quaternion LeftHandRotation = Quaternion.LookRotation(forward, up);
            LeftHandRotation.x = LeftHandRotation.x;
            LeftHandRotation.y = LeftHandRotation.y;
            LeftHandRotation.z = -LeftHandRotation.z;
            LeftHandRotation.w = -LeftHandRotation.w;
            gameObject.transform.rotation = LeftHandRotation;
            gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
            print("Pos: " +  pos);
            print("handMedPosition: " + handMedPositions[0]);
            //gameObject.transform.position = new Vector3(pos.x * scalingFactor, pos.y * scalingFactor, -pos.z * scalingFactor);
            gameObject.transform.position = new Vector3(handMedPositions[0].x, handMedPositions[0].y, handMedPositions[0].z);
        }
    }

    void calculateActiveHand() { 
        //if one right hand is closer to the middle of the coffee machine, it is "active" and can touch buttons
        Vector3 tmpHandPos = new Vector3();
        if (isRightHand)
        {
            tmpHandPos = GameObject.Find("model_hand_left").transform.position;
        }
        else
        {
            tmpHandPos = GameObject.Find("model_hand_right").transform.position;
        }

        var button1Pos = GameObject.Find("Cylinder_036").transform.position;
        if (euclideanDistance(tmpHandPos, button1Pos) > euclideanDistance(button1Pos, gameObject.transform.position))
        {
            computeDistanceAndColorButtons(handMedPositions);
        }
    }

    void makeHandsTransparent()
    {
        Color temp = new Color(gameObject.GetComponent<Renderer>().material.color.r, gameObject.GetComponent<Renderer>().material.color.g, gameObject.GetComponent<Renderer>().material.color.b, 0.5f);
        gameObject.GetComponent<Renderer>().material.color = temp;
    }

    //necessary?
    void computeLeapFrameMedian(bool isRightHand, Hand hand)
    {
        //var pos = body.Joints[TrackedJoint].Position;

        var fingers = hand.Fingers;
        var tmppos = new Vector();
        tmppos = hand.PalmPosition;
        handPositionsTimeSteps[currentFrame, 0, 0] = scalingFactor * tmppos.x;
        handPositionsTimeSteps[currentFrame, 0, 1] = scalingFactor * tmppos.y;
        handPositionsTimeSteps[currentFrame, 0, 2] = scalingFactor * tmppos.z;
        tmppos = hand.WristPosition;
        handPositionsTimeSteps[currentFrame, 1, 0] = scalingFactor * tmppos.x;
        handPositionsTimeSteps[currentFrame, 1, 1] = scalingFactor * tmppos.y;
        handPositionsTimeSteps[currentFrame, 1, 2] = scalingFactor * tmppos.z;
        tmppos = fingers[1].TipPosition;
        handPositionsTimeSteps[currentFrame, 2, 0] = scalingFactor * tmppos.x;
        handPositionsTimeSteps[currentFrame, 2, 1] = scalingFactor * tmppos.y;
        handPositionsTimeSteps[currentFrame, 2, 2] = scalingFactor * tmppos.z;
        tmppos = fingers[0].TipPosition;
        handPositionsTimeSteps[currentFrame, 3, 0] = scalingFactor * tmppos.x;
        handPositionsTimeSteps[currentFrame, 3, 1] = scalingFactor * tmppos.y;
        handPositionsTimeSteps[currentFrame, 3, 2] = scalingFactor * tmppos.z;
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
        Vector3 tmpvec = new Vector3(hand.PalmPosition.x, hand.PalmPosition.y, hand.PalmPosition.z);
        calculatePositions(tmpvec);
        calculateActiveHand();
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
            else
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
