using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bag : MonoBehaviour
{
    public List<int> sack = new List<int>();
    public TextMeshPro text;
    // Start is called before the first frame update
    public void InitialFill()
    {
        for(int y = 0; y < 4; y++)
        {
            for(int i = 0; i < 5; i++)
            {
                sack.Add(y);
            }
        }
        text.text = sack.Count + "";
    }

    public int draw()
    {
        int rand = Random.Range(0, sack.Count-1);
        int val = sack[rand];
        sack.Remove(rand);
        text.text = sack.Count + "";
        return val;
    }
}