using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Gem gemPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;

    public List<Gem> gems;

    public Vector2 mousePos;
    public Vector2 mouseDownPos;

    public bool isDraging;

    public Gem selectGem;
    public Gem swapGem;


    private void Awake()
    {
        TileGenerate();
    }

    private void Update()
    {
        Vector3 mouseOnWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2(Mathf.Round(mouseOnWorld.x), Mathf.Round(mouseOnWorld.y));

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = mousePos;
            isDraging = true;
            selectGem = gems.FirstOrDefault(g => (Vector2)g.transform.position == mouseDownPos);
        }

        if (isDraging)
        {
            if (mousePos != mouseDownPos)
            {
                swapGem = gems.FirstOrDefault(g => (Vector2)g.transform.position == mousePos);
                Swap(selectGem, swapGem);
                isDraging = false;
            }

        }
    }

    private void TileGenerate()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                var gem = Instantiate(gemPrefab, transform);
                gem.transform.localPosition = new Vector3(w, h, 0);
                gem.name = $"Gem_{w}_{h}";
                int rand = UnityEngine.Random.Range(0, 5);
                switch (rand)
                {
                    case 0:
                        gem.GetComponent<SpriteRenderer>().color = Color.green;
                        gem.type = GemType.green;
                        break;
                    case 1:
                        gem.GetComponent<SpriteRenderer>().color = Color.red;
                        gem.type = GemType.red;
                        break;
                    case 2:
                        gem.GetComponent<SpriteRenderer>().color = Color.yellow;
                        gem.type = GemType.yellow;
                        break;
                    case 3:
                        gem.GetComponent<SpriteRenderer>().color = Color.blue;
                        gem.type = GemType.blue;
                        break;
                    case 4:
                        gem.GetComponent<SpriteRenderer>().color = Color.white;
                        gem.type = GemType.white;
                        break;
                    default:
                        break;
                }
                gems.Add(gem);
            }
        }
    }

    private void Swap(Gem gem1, Gem gem2)
    {
        var pos1 = gem1.transform.position;
        gem1.transform.position = gem2.transform.position;
        gem2.transform.position = pos1;
        CheckMatch(gem1);
    }

    private void CheckMatch(Gem gem)
    {
        int x = (int)gem.transform.position.x;
        int y = (int)gem.transform.position.y;

        int rightMatchAmount = 0;
        for (int i = x + 1; i < width; i++)
        {
            var rightGem = gems.FirstOrDefault(g => (Vector2)g.transform.position == new Vector2(i, y));
            if (rightGem != null && rightGem.type == gem.type)
            {
                rightMatchAmount++;
            }
            else
            {
                break;
            }
        }

        int leftMatchAmount = 0;
        for (int i = x - 1; i >= 0; i--)
        {
            var leftGem = gems.FirstOrDefault(g => (Vector2)g.transform.position == new Vector2(i, y));
            if (leftGem != null && leftGem.type == gem.type)
            {
                leftMatchAmount++;
            }
            else
            {
                break;
            }
        }

        int upMatchAmount = 0;
        for (int i = y + 1; i < height; i++)
        {
            var upGem = gems.FirstOrDefault(g => (Vector2)g.transform.position == new Vector2(x, i));
            if (upGem != null && upGem.type == gem.type)
            {
                upMatchAmount++;
            }
            else
            {
                break;
            }
        }

        int downMatchAmount = 0;
        for (int i = y - 1; i >= 0; i--)
        {
            var downGem = gems.FirstOrDefault(g => (Vector2)g.transform.position == new Vector2(x, i));
            if (downGem != null && downGem.type == gem.type)
            {
                downMatchAmount++;
            }
            else
            {
                break;
            }
        }

        Debug.Log("horizon match " + (rightMatchAmount + leftMatchAmount + 1));
        Debug.Log("veritcal match " + (upMatchAmount + downMatchAmount + 1));
    }
}


