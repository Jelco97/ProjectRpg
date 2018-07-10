using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeightGround
{
    [System.Serializable]
    public struct TileHeight
    {
        public float[] Row;//position dans la ranger
    }

    public TileHeight[] HeightGroundData = new TileHeight[10];

    public void CleanHeight()
    {
        for (int i = 0; i < HeightGroundData.Length; i++)
            for (int x = 0; x < HeightGroundData.Length; x++)
            {
                HeightGroundData[i].Row[x] = 0;
            }
    }

    public void InitialisationRowArray(int size)
    {
        HeightGroundData = new TileHeight[size];
        for (int x = 0; x < size; x++)
            HeightGroundData[x].Row = new float[size];
    }

    /// <summary>
    /// Add or remove ellement of array
    /// </summary>
    /// <param name="i"></param>
    public void NewRowArray(int i)
    {
        int maxIndex = HeightGroundData.Length;

        TileHeight[] newAray = new TileHeight[i];
        for (int x = 0; x < i; x++)
        {
            newAray[x].Row = new float[i];
        }

        for(int y = 0; y < i; y++)
        {
            if (y >= maxIndex)
                continue;

            for(int x = 0; x < i; x++)
            {
                if (x >= maxIndex)
                    continue;

                newAray[y].Row[x] = HeightGroundData[y].Row[x];
            }
        }

        HeightGroundData = newAray;
    }
}
