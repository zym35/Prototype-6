using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tray : MonoBehaviour
{
    public GameObject[] tray;
    public int player;
    public TextMeshPro statusText;
    public Bag b;
    public Shuffle s;
    public Tray enemyTray;
    public GameObject attackMarble;
    public GameObject critMarble;
    public GameObject healMarble;
    public GameObject shuffleMarble;
    public float[] xpos = new float[4];
    public float ypos;
    // Start is called before the first frame update
    void Awake()
    {
        tray = new GameObject[4];
        b.InitialFill();
        draw(0, false);
        draw(1, false);
        draw(2, false);
        draw(3, false);
        if (player == 1)
        {
            setActive(true);
            setPlaying(true);
        }
        else if (player == 2)
        {
            setActive(false);
            setPlaying(false);
        }
        statusText.text = "Player 1 playing";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void draw(int slot, bool discarding)
    {
        GameObject m = tray[slot];
        tray[slot] = null;
        int newMarb = b.draw();
        GameObject marble = null;
        if (newMarb == 0)
        {
            marble = attackMarble;
        }
        else if (newMarb == 1)
        {
            marble = critMarble;
        }
        else if (newMarb == 2)
        {
            marble = healMarble;
        }
        else if (newMarb == 3)
        {
            marble = shuffleMarble;
        }
        Marble marbleInfo = marble.GetComponent<Marble>();
        marbleInfo.slot = slot;
        marbleInfo.t = this;
        marbleInfo.s = s;
        marbleInfo.type = newMarb;
        if (discarding)
        {
            setActive(true);
            enemyTray.setActive(false);
        }
        else
        {
            setActive(false);
            enemyTray.setActive(true);
        }
        tray[slot] = Instantiate(marble, new Vector3(xpos[slot], ypos, -1f), Quaternion.identity);
        bool isPlaying;
        if (m != null)
        {
            Marble m_Info = m.GetComponent<Marble>();
            isPlaying = m_Info.isPlaying;
            marbleInfo.isPlaying = !isPlaying;
            m_Info.isPlaying = false;
            m_Info.playerActive = false;
            Destroy(m);
        }
    }

    public void setActive(bool active)
    {
        for(int y = 0; y < tray.Length; y++)
        {
            if (tray[y] != null)
            {
                tray[y].GetComponent<Marble>().playerActive = active;
            }
        }
    }

    public void setPlaying(bool active)
    {
        for (int y = 0; y < tray.Length; y++)
        {
            if (tray[y] != null)
            {
                tray[y].GetComponent<Marble>().isPlaying = active;
            }
        }
    }
}