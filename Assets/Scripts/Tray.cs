using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tray : MonoBehaviour
{
    public Marble[] tray = new Marble[4];
    public int player;
    public TextMeshPro statusText;
    public Bag b;
    public Shuffle s;
    public Tray enemyTray;
    public Marble attackMarble;
    public Marble critMarble;
    public Marble healMarble;
    public Marble shuffleMarble;
    public float[] xpos = new float[4];
    public float ypos;
    // Start is called before the first frame update
    void Start()
    {
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
        Marble m = tray[slot];
        tray[slot] = null;
        int newMarb = b.draw();
        Marble marble = null;
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
        marble.slot = slot;
        marble.t = this;
        marble.s = s;
        marble.type = newMarb;
        tray[slot] = marble;
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
        Instantiate(marble, new Vector3(xpos[slot], ypos, 0), Quaternion.identity);
        bool isPlaying;
        if (m != null)
        {
            isPlaying = m.isPlaying;
            marble.isPlaying = !isPlaying;
            Destroy(m);
        }
    }

    public void setActive(bool active)
    {
        for(int y = 0; y < tray.Length; y++)
        {
            tray[y].playerActive = active;
        }
    }

    public void setPlaying(bool active)
    {
        for (int y = 0; y < tray.Length; y++)
        {
            tray[y].isPlaying = active;
        }
    }
}