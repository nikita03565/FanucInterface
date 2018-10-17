using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NetConnection : MonoBehaviour {

    public bool observerMode;
    public string message;
    NetworkStream stream;
    //const string Hostname="192.168.1.5";
    string Hostname = "192.168.0.177";
    //const int Port = 8882;
    int Port = 9090;
    TcpClient SocketConnection;
    Thread ReceiveThread;
    bool isConnected = false;
   
    void Start () {
        try
           
        {
            DontDestroyOnLoad(this.gameObject);
           // FindObjectsOfType<Button>()[0].onClick.RemoveAllListeners();
            //FindObjectsOfType<Button>()[0].onClick.AddListener(()=>Connect());
        }
        catch(UnityException Error)
        {
            Debug.Log("Thread init exception: " + Error);
        }
	}
    IEnumerator WaitingForConnect()
    {
        int sec = 0;
        while (sec < 5)
        {
            if (isConnected)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
                break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
                ++sec;
                Debug.Log(sec);
            }
        }
        

    }
    public void Connect()
    {
        try
        {
            Hostname = FindObjectsOfType<InputField>()[0].text;
            Port = int.Parse(FindObjectsOfType<InputField>()[1].text);
            Debug.Log("NET STARTED");
            ReceiveThread = new Thread(new ThreadStart(Listener));
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();
           
            StartCoroutine(WaitingForConnect());
           
              
        }
        catch (UnityException Error)
        {
            Debug.Log("Thread init exception: " + Error);
        }
    }
    void Getscene()
    {
        Sender(RobotCommands.GetSceneInf());
    }
    void Listener()
    {
        try
        {

            SocketConnection = new TcpClient(Hostname, Port);
            
            stream = SocketConnection.GetStream();
            if (stream.CanRead)
            {
                isConnected = true;
                Debug.Log("success");
            }
            byte[] buffer = new byte[50000];
            // using (NetworkStream stream = SocketConnection.GetStream())
            {
                int length;
                while (true)
                {
                    message = "";
                    if (!stream.CanRead)
                        Debug.Log("im  dead");
                   
                    if((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    { 
                        Debug.Log("Length of message= " + length);
                        var ByteMessage = new byte[length];
                      //  mutex.WaitOne();
                        System.Array.Copy(buffer, ByteMessage, length);
                      //  mutex.ReleaseMutex();
                        //any convert of message here!

                        message = ASCIIEncoding.ASCII.GetString(ByteMessage);
                        Debug.Log(message);
                        if (message != "")
                        {

                            
                            SceneManager.SceneSynchronization.Syncronization(message);
                            //StartCoroutine(InstantiateFromCam.Syncronization(message));
                            Debug.Log("sended to instantiate");
                        }

                        //Message in string here!\
                    }
                   
                }
            }
        }
        catch (SocketException Error)
        {
            Debug.Log("Socket Listener exception: " + Error);
        }
        //finally
        //{
            //SocketConnection.Close();
        //}
    }

  
    public void Sender(string Command)
    {
        Debug.Log("trying to send");
        Debug.Log(Command);
        if (SocketConnection==null)
        {
            Debug.Log("NULL");
            return;
        }
        try
        {
            if (!SocketConnection.Connected)
            {
                Debug.Log("UNCONNECT");
                return;
            }
            
            if (stream.CanWrite)
            {                
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
        //finally
        //{  
           // stream.Close();
           // SocketConnection.Close();
        //}  
    }

    public void OnQuit()
    {
        string MessageToServer = "{\"flag\": \"e\",\"name\": \"\",\"Scenario\": []}";

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
