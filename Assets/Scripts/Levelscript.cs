using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelscript : MonoBehaviour
{
    public Object levelBlueprint;
    public Vector2 levelSize;
    public int viewDistance;
    public int fogDistance;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        float cornerCoordinates = (this.viewDistance * 2) / 2 * levelSize.y;

        for (int i = this.viewDistance * 2; i >= 0; i--)
        {
            for (int j = this.viewDistance * 2; j >= 0; j--)
            {
                //Debug.Log("test");
                Instantiate(levelBlueprint, new Vector3(i * levelSize.y - cornerCoordinates, 0, j * levelSize.x - cornerCoordinates), Quaternion.Euler(new Vector3(0,0,0)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        outOfBounds();
    }

    void outOfBounds()
    {
        Vector3 teleportVector = new Vector3(0, 0, 0);

        if (this.player.transform.position.x > this.levelSize.x)
        {
            teleportVector += new Vector3(-this.levelSize.x, 0, 0);
        }
        if (this.player.transform.position.x < 0)
        {
            teleportVector += new Vector3(this.levelSize.x, 0, 0);
        }
        if (this.player.transform.position.z > this.levelSize.y)
        {
            teleportVector += new Vector3(0, 0, -this.levelSize.y);
        }
        if (this.player.transform.position.z < 0)
        {
            teleportVector += new Vector3(0, 0, this.levelSize.y);
        }

        this.player.transform.position += teleportVector;
    }
}
