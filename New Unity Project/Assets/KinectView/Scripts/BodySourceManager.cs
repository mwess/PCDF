using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;
    
    public Body[] GetData()
    {
        return _Data;
    }
    

    void Start () 
    {
        _Sensor = KinectSensor.GetDefault();
        //print("Default sensor is: " + _Sensor);

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();

            //print("Reader: " + _Reader);
            //print("Sensor is open: " + _Sensor.IsOpen);
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
            //print("Sensor is now open: " + _Sensor.IsOpen);
        }   
    }
    
    void Update () 
    {
        //print(_Reader);
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            //print(frame);
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                
                frame.GetAndRefreshBodyData(_Data);
                
                frame.Dispose();
                frame = null;
            }
        }    
    }
    
    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }
        
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            
            _Sensor = null;
        }
    }
}
