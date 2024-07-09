using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    public static void SaveShip(ShipData ship)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/"+ship.Name + "SRShip.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        ShipData data = ship;
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved");
    }

    public static ShipData LoadShip(ShipData ship)
    {
        string path = Application.persistentDataPath + "/" + ship.Name + "SRShip.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            //formatter.Deserialize(stream);
            object DeserializedStream = formatter.Deserialize(stream);
            stream.Close();
            ShipData data = (ShipData)DeserializedStream;
            return data;
        }else
        {
            Debug.LogWarning("Save not found at " + path + ", file created.");
            SaveShip(ship);
            return null;
        }
    }

    //public static void SaveInventory(Inventory inventory)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = Application.persistentDataPath + "/" + "Inventory.bal";
    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    InventoryData data = new InventoryData(inventory);
    //    formatter.Serialize(stream, data);
    //    stream.Close();
    //}

    //public static InventoryData LoadInventory()
    //{
    //    string path = Application.persistentDataPath + "/" + "Inventory.bal";
    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream stream = new FileStream(path, FileMode.Open);
    //        //formatter.Deserialize(stream);
    //        object DeserializedStream = formatter.Deserialize(stream);
    //        stream.Close();
    //        InventoryData data = (InventoryData)DeserializedStream;
    //        return data;
    //    }
    //    else
    //    {
    //        Debug.LogError("Inventory save not found at " + path);
    //        return null;
    //    }
    //}
}
