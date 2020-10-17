using Google.Protobuf;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class SocketManage
{
    /// <summary>
    /// 訊息包
    /// </summary>
    public class MessagePack
    {
        public int Len;//長度
        public int MsgID;//訊息ID
        public byte[] Data;//資料 可以解成Protocol Buffer格式
    }

    /// <summary>
    /// 接收的執行緒
    /// </summary>
    Thread ReceiveThread;
    /// <summary>
    /// socket
    /// </summary>
    Socket client;

    public bool OnLine
    {
        get
        {
            return client != null;
        }
    }

    /// <summary>
    /// 訊息佇列
    /// </summary>
    ConcurrentQueue<MessagePack> DataCQ = new ConcurrentQueue<MessagePack>();
    /// <summary>
    /// 主機名稱
    /// </summary>
    string Host= "0.tcp.ngrok.io";
    /// <summary>
    /// 阜號
    /// </summary>
    int Port =17306;

    public SocketManage(string host,int port)
    {
        Host = host;
        Port = port;
    }

    public void Init()
    {
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            client.Connect(Host, Port);
        }
        catch
        {
            close();
            return;
        }

        ReceiveThread = new Thread(new ThreadStart(Receive));
        ReceiveThread.IsBackground = true;
        ReceiveThread.Start();
    }
    /// <summary>
    /// 關閉socket
    /// </summary>
    public void close()
    {
        if (client == null)
        {
            return;
        }

        if (!client.Connected)
        {
            return;
        }

        client.Shutdown(SocketShutdown.Both);
        client.Close();
        client = null;
    }
    /// <summary>
    /// 接收訊息的業務
    /// </summary>
    void Receive()
    {
        int receivelength = 0;
        byte[] b = new byte[1024];

        while (true)
        {
            try
            {
                receivelength = client.Receive(b);
                if (receivelength > 0)
                {
                    byte[] gethead = new byte[8];
                    Array.Copy(b, gethead, gethead.Length);
                    MessagePack t = UnPack(gethead);
                    if (t.Len > 0)
                    {
                        Array.Copy(b, 8, t.Data, 0, t.Len);
                    }

                    Array.Clear(b, 0, b.Length);
                    DataCQ.Enqueue(t);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            catch (SocketException e)
            {
                break;
            }
            catch (Exception ex)
            {
                break;
            }
        }
        close();
    }
    /// <summary>
    /// 裝包
    /// </summary>
    byte[] Pack(int msgid, byte[] data)
    {
        byte[] resultdata = new byte[data.Length + 8];

        try
        {
            using (MemoryStream ms = new MemoryStream(resultdata))
            {
                ms.Write(BitConverter.GetBytes(data.Length), 0, 4);//寫入長度
                ms.Write(BitConverter.GetBytes(msgid), 0, 4);//寫入訊息id
                ms.Write(data, 0, data.Length);//寫入data
            }
        }
        catch (Exception ex)
        {
            return new byte[0];
        }

        return resultdata;
    }
    /// <summary>
    /// 解包
    /// </summary>
    MessagePack UnPack(byte[] headdata)
    {
        int MsgLength = 0;
        int MsgID = 0;

        try
        {
            MsgLength = BitConverter.ToInt32(headdata, 0);
            MsgID = BitConverter.ToInt32(headdata, 4);
        }
        catch (Exception ex)
        {

        }

        return new MessagePack() { Len = MsgLength, MsgID = MsgID, Data = new byte[MsgLength] };
    }

    /// <summary>
    /// 傳送訊息
    /// </summary>
    public void SendMsg(int msgid, IMessage msg)
    {
        byte[] m = Pack(msgid, HyanProto.Marshal(msg));
        client.Send(m);
    }

    /// <summary>
    /// 取得訊息
    /// </summary>
    public MessagePack GetData()
    {
        return DataCQ.TryDequeue(out MessagePack m) ? m : null;
    }
}
