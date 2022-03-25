using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// XML data persistence manager
/// Mainly contains save and load two functions
/// </summary>
public class XmlMgr : Singleton<XmlMgr>
{
    /// <summary>
    ///Save data
    /// </summary>
    /// <param name="data">the data you want to save</param>
    /// <param name="fileName">the file you want to put data in</param>
    public void SaveData(object data, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".xml"; //path of the file
        using (StreamWriter writer = new StreamWriter(path)) //open write data stream
        {
            XmlSerializer s = new XmlSerializer(data.GetType()); //serialization
            s.Serialize(writer, data);
        }
    }

    /// <summary>
    ///Load data
    /// </summary>
    /// <param name="type">the type of object you want to load</param>
    /// <param name="fileName">the file you want to read data from</param>
    /// <returns></returns>
    public object LoadData(Type type, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".xml"; //default path
        if (!File.Exists(path)) //if there is no default path
        {
            path = Application.streamingAssetsPath + "/" + fileName + ".xml"; //read from initialization file
            if (!File.Exists(path)) //if there is no initialization file as well
            {
                return Activator.CreateInstance(type); //return a null object according to the type
            }
        }

        using(StreamReader reader = new StreamReader(path)) //open read data stream
        {
            XmlSerializer s = new XmlSerializer(type); //deserialization
            return s.Deserialize(reader);
        }
    }
    
    
}
