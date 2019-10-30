using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxDebug : MonoBehaviour
{
    [SerializeField]
    private TileScript goal;
    [SerializeField]
    private TileScript spawn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTile();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ax.GetPath(spawn.GridPosition);
        }
    }

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);

            if(hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();
                if (tmp != null)
                {
                    if(spawn == null)
                    {
                        spawn = tmp;
                        spawn.Debugging = true;
                        spawn.SpriteRenderer.color = Color.red;
                    }else if(goal == null)
                    {
                        goal = tmp;
                        goal.Debugging = true;
                        goal.SpriteRenderer.color = Color.green;
                    }

                }
            }
        }
    }


    public void DebugPath(HashSet<Node> openList)
    {
        foreach(Node node in openList)
        {
            node.TileRef.SpriteRenderer.color = Color.cyan;
        }
    }
}
