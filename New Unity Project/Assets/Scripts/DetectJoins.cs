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
                print("Tracked");
                var pos = body.Joints[TrackedJoint].Position;
                gameObject.transform.position = new Vector3(pos.X * multiplier, pos.Y * multiplier, pos.Z * multiplier);
                //Object.FindObjectOfType(typeof(MonoBehaviour));
            }
            
        }
	}
}
