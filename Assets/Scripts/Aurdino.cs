using System.IO.Ports;
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
        serialPort.ReadTimeout = 10; // Prevents Unity from freezing if no data arrives

        try
        {
            serialPort.Open();
            Debug.Log("Serial Port Opened");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
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

