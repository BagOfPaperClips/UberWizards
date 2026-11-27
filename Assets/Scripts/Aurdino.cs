using System;
using System.IO.Ports;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;
public class Aurdino : MonoBehaviour
{

    public string portName = "COM4"; // Check Arduino IDE > Tools > Port
    public int baudRate = 19200;

    private SerialPort serialPort;
    public string latestMessage;

    public float roll;
    public float pitch;
    public float yaw;
    public float Xvalues;
    public float Yvalues;
    public static int button1;
    public static int button2;


    public float NEWXvalues;
    public float NEWYvalues;

    public static float newYaw;
    public float yawSet;
    public float xSet;
    public float ySet;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 50; // Prevents Unity from freezing if no data arrives

        try
        {
            serialPort.Open();
            Debug.Log("Serial Port Opened");
        }
        catch (System.Exception e)
        {
            //Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // Reads the incoming string up to the newline character
                latestMessage = serialPort.ReadLine();
                //Debug.Log("Received: " + latestMessage);

                string[] sections = latestMessage.Trim().Split(",");
                if (sections.Length != 7) return;

                roll = float.Parse(sections[0]);
                pitch = float.Parse(sections[1]);
                yaw = float.Parse(sections[2]);
                Xvalues = float.Parse(sections[3]);
                Yvalues = float.Parse(sections[4]);
                button1 = int.Parse(sections[5]);
                button2 = int.Parse(sections[6]);

                //Debug.Log("roll: " + roll+ "    pitch: " + pitch+ "     yaw: " + yaw+ "     XValues: " + Xvalues + "     Yvalues: " + Yvalues+"     button1 " + button1+ "      button2 " + button2);

                //Debug.Log("button1 " + button1);
                //Debug.Log("button2 " + button2);

            }
            catch (System.TimeoutException)
            {
                // Expected when no new line is ready during the current frame
            }
            if(button1 == 1 && button2 == 1)
            {
                Debug.Log("BOTH ARE CLICKED");
                yawSet = yaw;
                xSet = Xvalues;
                ySet = Yvalues;
            }

            NEWXvalues = xSet - Xvalues;
            NEWYvalues = ySet - Yvalues;
            newYaw = yawSet - yaw;
            //Debug.Log("Yaw: " + newYaw);
            //Debug.Log("XValues: " + NEWXvalues + "   YValues: " + NEWYvalues);

            if(newYaw < -4)
            {
                //Debug.Log("move LEFT");
            }
            if (newYaw > 4)
            {
                //Debug.Log("move RIGHT");
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
