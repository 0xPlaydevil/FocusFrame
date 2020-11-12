using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.Common;
using System;

public class CamFocusData : MonoBehaviour
{
    public DbManager dbm = null;
    public CamFocus focusCom = null;
    public string tableName = "NodeTable";
    public string nodeKey= "NodeID";
    public Dictionary<string, Orientation> oriens = new Dictionary<string, Orientation>();
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Assert(focusCom, "Inspector check: focusCom is null!", this);
    }

    public void FocusNode(string nodeID)
    {
        Orientation orien = oriens[nodeID];
        focusCom.Focus(orien.position, orien.eulerAngles);
    }

    public void FillOriens()
    {
        oriens.Clear();
        string[] cols = new string[Orientation.colNames.Length + 1];
        cols[0] = nodeKey;
        Orientation.colNames.CopyTo(cols, 1);
        var reader = dbm.db.ReadFullTable(tableName, cols);
        while (reader.Read())
        {
            oriens.Add(reader[nodeKey] as string, new Orientation(reader));
        }
    }

    public void SaveOriens()
    {

    }

    public void SetNode(string nodeID, Orientation orien)
    {
        oriens[nodeID] = orien;
        dbm.db.UpdateValues(tableName,Orientation.colNames,orien.ToStrArray(),nodeKey,"=",nodeID);
    }

    public void SetNode(string nodeID, Vector3 pos, Vector3 eul)
    {
    	SetNode(nodeID,new Orientation(pos,eul));
    }

    void OnEnable()
    {
    	FillOriens();
    }
}

public class Orientation
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public static string[] colNames = new string[6] { "PosX", "PosY", "PosZ", "EulY", "EulY", "EulY" };

    public Orientation(float px, float py, float pz, float ex, float ey, float ez)
    {
        position.Set(px, py, pz);
        eulerAngles.Set(ex, ey, ez);
    }

    public Orientation(Vector3 pos, Vector3 eul)
    {
        position = pos;
        eulerAngles = eul;
    }

    public Orientation(DbDataReader reader) : this((float)reader[colNames[0]], (float)reader[colNames[1]], (float)reader[colNames[2]], (float)reader[colNames[3]], (float)reader[colNames[4]], (float)reader[colNames[5]])
    {
    }

    public string[] ToStrArray()
    {
        return new string[6] { position.x.ToString(), position.y.ToString(), position.z.ToString(), eulerAngles.x.ToString(), eulerAngles.y.ToString(), eulerAngles.z.ToString() };
    }
}
