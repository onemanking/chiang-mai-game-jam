using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#region Enum

public enum TileDirection
{
    Up,
    Down,
    Left,
    Right
}

#endregion

public sealed class CGlobal_TilemapManager : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    


#pragma warning restore 0649
    #endregion

    #region Variable - Property

    static CGlobal_TilemapManager Instance
    {
        get
        {
            if (m_hInstance == null)
                SpawnThisManager();

            return m_hInstance;
        }
    }

    #endregion

    static CGlobal_TilemapManager m_hInstance;

    Tilemap m_hMainTilemap;

    #endregion

    #region Base - Mono

    private void Awake()
    {
        if(m_hInstance == null)
        {
            m_hInstance = this;
        }
        else if(m_hInstance != this)
        {
            Destroy(this);
            return;
        }

        if(m_hMainTilemap == null)
        {
            m_hMainTilemap = FindObjectOfType<Tilemap>();

            if(m_hMainTilemap == null)
                Debug.LogError("Please set main tilemap to this manager!");
        }
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 GetClickTilePosition()
    {
        return Instance.MainGetClickTilePosition();
    }

    /// <summary>
    /// 
    /// </summary>
    Vector3 MainGetClickTilePosition()
    {
        Vector3 vMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return MainGetTilePosition(vMousePos);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 GetTilePosition(Vector3 vPosition)
    {
        return Instance.MainGetTilePosition(vPosition);
    }

    /// <summary>
    /// 
    /// </summary>
    Vector3 MainGetTilePosition(Vector3 vPosition)
    {
        Vector3Int vTemp = m_hMainTilemap.WorldToCell(vPosition);
        Vector3 vResult = m_hMainTilemap.CellToWorld(vTemp);
        vResult.y += m_hMainTilemap.tileAnchor.y * 0.5f;

        vResult.z = 0;

        return vResult;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 GetNextTileInThisDirection(Vector3 vPosition,TileDirection eDirection)
    {
        return Instance.MainGetNextTileInThisDirection(vPosition,eDirection);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 GetNextTileInThisDirection(Vector3 vPosition, TileDirection eDirection,int nTileCount)
    {
        return Instance.MainGetNextTileInThisDirection(vPosition, eDirection,nTileCount);
    }

    /// <summary>
    /// 
    /// </summary>
    Vector3 MainGetNextTileInThisDirection(Vector3 vPosition, TileDirection eDirection,int nTileCount = 1)
    {
        var vPos = GetTilePosition(vPosition);

        switch (eDirection)
        {
            case TileDirection.Up:                
                vPos.x += m_hMainTilemap.tileAnchor.x * nTileCount;
                vPos.y += (m_hMainTilemap.tileAnchor.y * 0.5f) * nTileCount;
                break;

            case TileDirection.Down:
                vPos.x -= m_hMainTilemap.tileAnchor.x * nTileCount;
                vPos.y -= (m_hMainTilemap.tileAnchor.y * 0.5f) * nTileCount;
                break;

            case TileDirection.Left:
                vPos.x -= m_hMainTilemap.tileAnchor.x * nTileCount;
                vPos.y += (m_hMainTilemap.tileAnchor.y * 0.5f) * nTileCount;
                break;

            case TileDirection.Right:
                vPos.x += m_hMainTilemap.tileAnchor.x * nTileCount;
                vPos.y -= (m_hMainTilemap.tileAnchor.y * 0.5f) * nTileCount;
                break;
        }

        return vPos;
    }

    #endregion

    #region Helper

    /// <summary>
    /// 
    /// </summary>
    static void SpawnThisManager()
    {
        var hGO = new GameObject();
        hGO.AddComponent<CGlobal_TilemapManager>();
        hGO.name = "Tilemap Manager";
    }

    #endregion
}
