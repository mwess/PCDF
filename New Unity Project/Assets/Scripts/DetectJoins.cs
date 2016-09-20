using UnityEngine;
using System.Collections;
using System;
using Windows.Kinect;

public class DetectJoins : MonoBehaviour {

    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager BodyManager;
    private Body[] bodies;
    public float multiplier = 10f;
    Mesh mesh;


    // Use this for initialization
    void Start ()
    {
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
                    var pos_jointHandLeft = body.Joints[JointType.HandLeft].Position;

                    var pos_jointWristLeft = body.Joints[JointType.WristLeft].Position;

                    var pos_jointTipLeft = body.Joints[JointType.HandTipLeft].Position;

                    var pos_jointThumbLeft = body.Joints[JointType.ThumbLeft].Position;



                    

                } else if ( TrackedJoint.ToString() == "HandRight")
                {
                    var pos_jointHandRight = body.Joints[JointType.HandRight].Position;
                    var pos_jointWristRight = body.Joints[JointType.WristRight].Position;
                    var pos_jointTipRight = body.Joints[JointType.HandTipRight].Position;
                    var pos_jointThumbRight = body.Joints[JointType.ThumbRight].Position;
                }

                gameObject.transform.position = new Vector3(pos.X * multiplier, pos.Y * multiplier, pos.Z * multiplier);
                //Object.FindObjectOfType(typeof(MonoBehaviour));
            }
            
        }
	}
}
