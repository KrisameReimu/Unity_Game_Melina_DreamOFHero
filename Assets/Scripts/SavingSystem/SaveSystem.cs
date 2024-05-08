using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sdgame";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("File saved");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.sdgame";

        if(File.Exists(path))
        {
            Debug.Log("File exists");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close() ;

            return data;
        }
        else
        {
            Debug.Log("Save file not found in "+path);
            return null;
        }
    }
}
