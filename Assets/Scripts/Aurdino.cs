using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
public class Aurdino : MonoBehaviour
{

    public string portName = "COM4"; // Check Arduino IDE > Tools > Port
    public int baudRate = 19200;

    private SerialPort serialPort;
    public string latestMessage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 200; // Prevents Unity from freezing if no data arrives

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
                Debug.Log("Received: " + latestMessage);

                string[] sections = latestMessage.Split(',');
                //if (sections.Length != 7) return;

                float roll = float.Parse(sections[0]);
                float pitch = float.Parse(sections[1]);
                float yaw = float.Parse(sections[2]);
                float Xvalues = float.Parse(sections[3]);
                float Yvalues = float.Parse(sections[4]);
                float button1 = float.Parse(sections[5]);
                float button2 = float.Parse(sections[6]);

                Debug.Log("roll: " + roll+ "    pitch: " + pitch+ "     yaw: " + yaw+ "     XValues: " + Xvalues + "     Yvalues: " + Yvalues+"     button1 " + button1+ "      button2 " + button2);

                //Debug.Log("button1 " + button1);
                //Debug.Log("button2 " + button2);

            }
            catch (System.TimeoutException)
            {
                // Expected when no new line is ready during the current frame
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
