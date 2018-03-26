using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Net;
using System.Net.Sockets;
#else
using System;
using System.IO;
using Windows.Networking.Sockets;
#endif

public class TouchOscServer : MonoBehaviour
{
    [HideInInspector] public TouchOscServer instance;

    [System.Serializable] public class OnMessageCallback : UnityEvent<Osc.Message> { }
    [SerializeField] private OnMessageCallback onMessageCallback;
    [SerializeField] int listenPort = 3333;

    Osc.Parser osc_ = new Osc.Parser();

#if UNITY_EDITOR
    UdpClient udpClient_;
    IPEndPoint endPoint_;

    void Start()
    {
        instance = this;
        endPoint_ = new IPEndPoint(IPAddress.Any, listenPort);
        udpClient_ = new UdpClient(endPoint_);
    }

    void Update()
    {
        while (udpClient_.Available > 0)
        {
            var data = udpClient_.Receive(ref endPoint_);
            osc_.FeedData(data);
        }

        while (osc_.MessageCount > 0)
        {
            var msg = osc_.PopMessage();
            onMessageCallback.Invoke(msg);
        }
    }
#else
    DatagramSocket socket_;
    object lockObject_ = new object();

    const int MAX_BUFFER_SIZE = 1024;
    byte[] buffer = new byte[MAX_BUFFER_SIZE];

    async void Start()
    {
        try {
            socket_ = new DatagramSocket();
            socket_.MessageReceived += OnMessage;
            await socket_.BindServiceNameAsync(listenPort.ToString());
        } catch (System.Exception e) {
            Debug.LogError(e.ToString());
        }
    }

    void Update()
    {
        lock (lockObject_) {
            while (osc_.MessageCount > 0) {
                var msg = osc_.PopMessage();
                onMessageCallback.Invoke(msg);
            }
        }
    }

    async void OnMessage(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
    {
        using (var stream = args.GetDataStream().AsStreamForRead()) {
            await stream.ReadAsync(buffer, 0, MAX_BUFFER_SIZE);
            lock (lockObject_) {
                osc_.FeedData(buffer);
            }
        }
    }
#endif
}