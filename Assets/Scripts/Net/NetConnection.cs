using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NetConnection : MonoBehaviour {

    public string message;
    public FanucScript Fanuc;
    NetworkStream stream;
    //const string Hostname="192.168.1.5";
    const string Hostname = "192.168.1.106";
    const int Port = 8880;
    //const int Port = 9090;
    TcpClient SocketConnection;
    Thread ReceiveThread;
 
   
    Mutex mutex = new Mutex();
    // Use this for initialization
   
    void Start () {
        try
        {
            Debug.Log("NET STARTED");
            ReceiveThread = new Thread(new ThreadStart(Listener));
            ReceiveThread.IsBackground = true;
            
            ReceiveThread.Start();
        }
        catch(UnityException Error)
        {
            Debug.Log("Thread init exception: " + Error);
        }
	}
    void Listener()
    {
        try
        {
            SocketConnection = new TcpClient(Hostname, Port);
            
            stream = SocketConnection.GetStream();
            if(stream.CanRead)
                 Debug.Log("success");
            byte[] buffer = new byte[4096];
            // using (NetworkStream stream = SocketConnection.GetStream())
            {
                int length;
                while (true)
                {
                    if (!stream.CanRead)
                        Debug.Log("im  dead");
                    while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        var ByteMessage = new byte[length];
                      //  mutex.WaitOne();
                        System.Array.Copy(buffer, ByteMessage, length);
                      //  mutex.ReleaseMutex();
                        //any convert of message here!

                        message += ASCIIEncoding.ASCII.GetString(ByteMessage);
                        Debug.Log(message);
                        //Message in string here!\
                    }
                }
            }
        }
        catch (SocketException Error)
        {
            Debug.Log("Socket Listener exception: " + Error);
        }
        finally
        {
            SocketConnection.Close();
        }
    }

   // public void doMessage( string MessageToServer)
    //{
        //for (int i = 0; i < 6; ++i)
        //{
        //    MessageToServer += " " + Fanuc.jointAngles[i].ToString();
        //}
        //MessageToServer += " 0 ";
      //  Debug.Log(MessageToServer);
   // }
    public void Sender(string Command)
    {
        Debug.Log("trying to send");
        Debug.Log(Command);
        if (SocketConnection==null)
        {
            return;
        }
        try
        {
            if (!SocketConnection.Connected)
                return;
            
            if (stream.CanWrite)
            {
                //Command = "m";
                //for (int i = 0; i < 6; ++i)
                //{
                //    Command += " " + Fanuc.jointAngles[i].ToString();
                //}
                //Command += " 0 ";
                
                byte[] ByteMessageToServer = Encoding.ASCII.GetBytes(Command);
                stream.Write(ByteMessageToServer, 0, ByteMessageToServer.Length);
                Debug.Log("sended");
            }        
            else Debug.Log("stream cant write");
        }
        catch(SocketException Error)
        {
            Debug.Log("Socket Sender Error: " + Error);
        }
        finally
        {  
           // stream.Close();
           // SocketConnection.Close();
        }  
    }

    public void OnQuit()
    {
        string MessageToServer = "{\"flag\": \"e\",\"Scenario\": [{\"parallel\":\"False\", \"name\": \"\",\"time\":\"0\",\"energy\":\"0\", \"command\": \"\"}]}";

        Debug.Log(MessageToServer);
        byte[] ByteMessageToServer = Encoding.ASCII.GetBytes(MessageToServer);
        stream.Write(ByteMessageToServer, 0, ByteMessageToServer.Length);
        Debug.Log("sended Exit message");
    }

    void OnApplicationQuit()
    {
        OnQuit();

        stream.Close();
        SocketConnection.Close();
        ReceiveThread.Abort(); 
    }
}
