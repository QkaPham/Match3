using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gemSprite;
    [SerializeField] private SpriteRenderer selectBoder;

    public GemType type;
    public Vector3 Positon
    {
        get
        {
            return Vector3Int.RoundToInt(transform.position);
        }
        set
        {
            transform.position = value;
        }
    }

    public bool IsNeibour(Gem gem)
    {
        if (gem.transform.position.x == transform.position.x - 1 && gem.transform.position.y == transform.position.y)
        {
            return true;
        }
        if (gem.transform.position.x == transform.position.x + 1 && gem.transform.position.y == transform.position.y)
        {
            return true;
        }
        if (gem.transform.position.y == transform.position.y - 1 && gem.transform.position.x == transform.position.x)
        {
            return true;
        }
        if (gem.transform.position.y == transform.position.y + 1 && gem.transform.position.x == transform.position.x)
        {
            return true;
        }
        return false;
    }

    public void MoveTo(Vector3 toPosition)
    {
        transform.DOMove(toPosition, .2f);
    }

    public void RandomType()
    {
        GemType randType = (GemType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(GemType)).Length);
        type = randType;
        gemSprite.color = GemsProfile.gemsDataDic[randType];
        //switch (randType)
        //{
        //    case GemType.green:
        //        gemSprite.color = Color.green;
        //        break;
        //    case GemType.red:
        //        gemSprite.color = Color.red;
        //        break;
        //    case GemType.yellow:
        //        gemSprite.color = Color.yellow;
        //        break;
        //    case GemType.blue:
        //        gemSprite.color = Color.blue;
        //        break;
        //    case GemType.white:
        //        gemSprite.color = Color.white;
        //        break;
        //    default:
        //        break;
        //}
    }

    public void RandomTypeExclude(List<GemType> excludetypes)
    {
        List<GemType> GemTypes = new List<GemType>(GemsProfile.Gems);
        excludetypes.ForEach(excludetype => GemTypes.Remove(excludetype));

        GemType randType = GemTypes[UnityEngine.Random.Range(0, GemTypes.Count)];
        type = randType;
        gemSprite.color = GemsProfile.gemsDataDic[randType];
        //switch (randType)
        //{
        //    case GemType.green:
        //        gemSprite.color = Color.green;
        //        break;
        //    case GemType.red:
        //        gemSprite.color = Color.red;
        //        break;
        //    case GemType.yellow:
        //        gemSprite.color = Color.yellow;
        //        break;
        //    case GemType.blue:
        //        gemSprite.color = Color.blue;
        //        break;
        //    case GemType.white:
        //        gemSprite.color = Color.white;
        //        break;
        //    default:
        //        break;
        //}
    }

    public void Select()
    {
        selectBoder.enabled = true;
    }

    public void Deselect()
    {
        selectBoder.enabled = false;
    }
}
