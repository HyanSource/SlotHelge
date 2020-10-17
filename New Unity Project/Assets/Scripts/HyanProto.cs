using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using System.IO;

public class HyanProto
{
    public static byte[] Marshal<T>(T t) where T : IMessage
    {
        using (MemoryStream ms = new MemoryStream())
        {
            t.WriteTo(ms);
            return ms.ToArray();
        }
    }

    public static T UnMarshal<T>(byte[] b) where T : class, IMessage, new()
    {
        return new T().Descriptor.Parser.ParseFrom(b) as T;
    }
}
