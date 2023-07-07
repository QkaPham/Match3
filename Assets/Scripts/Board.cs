using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Gem gemPrefab;
    [SerializeField] private int boardWidth;
    [SerializeField] private int boardHeight;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private int turn;
    [SerializeField] private bool isWaitingInput;
    [SerializeField] private float swapDuration = .5f;
    [SerializeField] private float fillDuration = .5f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    public int Turn
    {
        get
        {
            return turn;
        }
        set
        {
            turn = value;
            gamePanel.UpdateTurn(turn);
        }
    }

    [SerializeField] private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            gamePanel.UpdateScore(score);
        }
    }

    private List<Gem> gems = new List<Gem>();

    private Vector2 mousePos;
    private Vector2 mouseDownPos;

    private bool isDraging;

    [SerializeField] private Gem selectGem;
    public Gem SelectGem
    {
        get
        {
            return selectGem;
        }
        set
        {
            if (value == null)
            {
                if (selectGem != null)
                {
                    selectGem.Deselect();
                    selectGem = value;
                }
            }
            else
            {
                selectGem = value;
                selectGem.Select();
            }
        }
    }
    //[SerializeField] private Gem swapGem;

    public void Init()
    {
        gems.ForEach(gem => Destroy(gem.gameObject));
        gems.Clear();
        GemGenerate();
        Turn = 10;
        Score = 0;
        isWaitingInput = true;
    }

    private void Update()
    {
        Vector3 mouseOnWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2(Mathf.Round(mouseOnWorld.x), Mathf.Round(mouseOnWorld.y));

        if (isWaitingInput)
        {
            if (turn == 0)
            {
                isWaitingInput = false;
                gameOverPanel.UpdateScore(score);
                gameManager.GameOver();
            }

            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = mousePos;
                isDraging = true;
                if (SelectGem == null)
                {
                    SelectGem = GetGem(mouseDownPos);
                }
                else
                {
                    var swapGem = GetGem(mousePos);
                    if (swapGem != null)
                    {
                        if (swapGem.IsNeibour(SelectGem))
                        {
                            Swap(SelectGem, swapGem);
                            isDraging = false;
                        }
                        SelectGem = null;
                        // swapGem = null;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDraging = false;
            }

            if (isDraging)
            {
                if (mousePos != mouseDownPos)
                {
                    var swapGem = GetGem(mousePos);
                    Swap(SelectGem, swapGem);
                    SelectGem = null;
                    //swapGem = null;
                    isDraging = false;
                }

            }
        }
    }

    private Gem GetGem(float x, float y, float z = 0f)
    {
        return GetGem(new Vector3(x, y, z));
    }

    private Gem GetGem(Vector3 position)
    {
        return gems.FirstOrDefault(gem => gem.Positon == position);
    }

    public void TileGenerate()
    {
        for (int w = 0; w < boardWidth; w++)
        {
            for (int h = 0; h < boardHeight; h++)
            {
                Instantiate(tilePrefab, new Vector3(w, h, 0), Quaternion.identity, transform);
            }
        }
    }

    private void GemGenerate()
    {
        for (int w = 0; w < boardWidth; w++)
        {
            for (int h = 0; h < boardHeight; h++)
            {
                var newGem = CreateNewGem(new Vector3(w, h, 0), fillDuration, true);
                gems.Add(newGem);
            }
        }
    }

    private Gem CreateNewGem(Vector3 position, float duration, bool preventRepeating = false)
    {
        var initPos = new Vector3(position.x, position.y + boardHeight, 0);
        var gem = CreateNewGem(initPos, preventRepeating);
        gem.transform.DOMove(position, duration);
        return gem;
    }

    private Gem CreateNewGem(Vector3 position, bool preventRepeating = false)
    {
        var gem = Instantiate(gemPrefab, position, Quaternion.identity, transform);
        gem.name = $"{position.x}_{position.y}";

        if (preventRepeating)
        {
            List<GemType> excludeTypes = new List<GemType>();

            if (position.x >= 2)
            {
                if (GetGem(position.x - 1, position.y).type == GetGem(position.x - 2, position.y).type)
                {
                    excludeTypes.Add(GetGem(position.x - 1, position.y).type);
                }
            }

            if (position.y >= 2 + boardHeight)
            {
                if (GetGem(position.x, position.y - 1).type == GetGem(position.x, position.y - 2).type)
                {
                    excludeTypes.Add(GetGem(position.x, position.y - 1).type);
                }
            }

            if (excludeTypes.Count > 0)
            {
                gem.RandomTypeExclude(excludeTypes);
            }
            else
            {
                gem.RandomType();
            }
        }
        else
        {
            gem.RandomType();
        }
        return gem;
    }

    private void Swap(Gem gem1, Gem gem2)
    {
        audioManager.PlaySFX(SFXID.Swap);
        isWaitingInput = false;
        Turn--;
        var pos1 = gem1.Positon;
        var pos2 = gem2.Positon;

        Sequence seq = DOTween.Sequence();
        seq.Append(gem1.transform.DOMove(pos2, swapDuration));
        seq.Join(gem2.transform.DOMove(pos1, swapDuration));
        seq.OnComplete(() =>
        {
            if (!Matching(new List<Gem> { gem1, gem2 }))
            {
                Sequence seq1 = DOTween.Sequence();
                seq1.Append(gem1.transform.DOMove(pos1, swapDuration));
                seq1.Join(gem2.transform.DOMove(pos2, swapDuration));
                seq1.OnComplete(() => isWaitingInput = true);
            }
        });
    }

    private bool Matching(List<Gem> checkGems)
    {
        if (HasMatch(checkGems, out var matchGems))
        {
            audioManager.PlaySFX(SFXID.Match);
            DestroyMatch(matchGems);
            RefillBoard(matchGems);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool HasMatch(List<Gem> checkGems, out List<Gem> matchGems)
    {
        matchGems = new List<Gem>();
        foreach (var gem in checkGems)
        {
            int x = (int)gem.Positon.x;
            int y = (int)gem.Positon.y;

            List<Gem> horizontalMatch = new List<Gem> { gem };
            List<Gem> verticalMatch = new List<Gem> { gem };

            for (int i = x + 1; i < boardWidth; i++)
            {
                var rightGem = gems.FirstOrDefault(g => (Vector2)g.Positon == new Vector2(i, y));
                if (rightGem != null && rightGem.type == gem.type)
                {
                    horizontalMatch.Add(rightGem);
                }
                else
                {
                    break;
                }
            }

            for (int i = x - 1; i >= 0; i--)
            {
                var leftGem = gems.FirstOrDefault(g => (Vector2)g.Positon == new Vector2(i, y));
                if (leftGem != null && leftGem.type == gem.type)
                {
                    horizontalMatch.Add(leftGem);
                }
                else
                {
                    break;
                }
            }

            for (int i = y + 1; i < boardHeight; i++)
            {
                var upGem = gems.FirstOrDefault(g => (Vector2)g.Positon == new Vector2(x, i));
                if (upGem != null && upGem.type == gem.type)
                {
                    verticalMatch.Add(upGem);
                }
                else
                {
                    break;
                }
            }

            for (int i = y - 1; i >= 0; i--)
            {
                var downGem = gems.FirstOrDefault(g => (Vector2)g.Positon == new Vector2(x, i));
                if (downGem != null && downGem.type == gem.type)
                {
                    verticalMatch.Add(downGem);
                }
                else
                {
                    break;
                }
            }

            if (horizontalMatch.Count >= 3)
            {
                matchGems.AddRange(horizontalMatch);
            }

            if (verticalMatch.Count >= 3)
            {
                matchGems.AddRange(verticalMatch);
            }
        }
        matchGems = matchGems.Distinct().ToList();

        return matchGems.Count() > 0;
    }

    private void DestroyMatch(List<Gem> matchGems)
    {
        foreach (var gem in matchGems)
        {
            gems.Remove(gem);
            Destroy(gem.gameObject);
            Score++;
        }
    }

    private void RefillBoard(List<Gem> destroyGems)
    {
        destroyGems = destroyGems.OrderByDescending(gem => gem.Positon.y).ToList();
        Dictionary<Gem, int> moveDistances = new Dictionary<Gem, int>();

        foreach (var destroyGem in destroyGems)
        {
            int fillHeight = boardHeight;
            while (gems.FirstOrDefault(gem => gem.Positon.y == fillHeight && destroyGem.Positon.x == gem.Positon.x) != null)
            {
                fillHeight++;
            }
            var fillPos = new Vector3(destroyGem.Positon.x, fillHeight, 0);
            var fillGem = CreateNewGem(fillPos);
            gems.Add(fillGem);
        }

        foreach (var destroyGem in destroyGems)
        {
            var aboveGems = gems.Where(gem => gem.Positon.x == destroyGem.Positon.x && gem.Positon.y > destroyGem.Positon.y).ToList();
            foreach (var aboveGem in aboveGems)
            {
                if (moveDistances.ContainsKey(aboveGem))
                {
                    moveDistances[aboveGem] = moveDistances[aboveGem] + 1;
                }
                else
                {
                    moveDistances.Add(aboveGem, 1);
                }
            }
        }

        Sequence seq = DOTween.Sequence();
        foreach (var moveDistance in moveDistances)
        {
            var pos = moveDistance.Key.Positon;
            var moveToPos = new Vector3(pos.x, pos.y - moveDistance.Value, pos.z);
            seq.Join(moveDistance.Key.transform.DOMove(moveToPos, fillDuration));
        }

        seq.OnComplete(() =>
        {
            if (!Matching(moveDistances.Keys.ToList()))
            {
                isWaitingInput = true;
            }
        });
    }
}


