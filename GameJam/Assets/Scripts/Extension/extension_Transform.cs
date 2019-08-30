using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class extension_Transform 
{
    /// <summary>
    /// 
    /// </summary>
    public static Vector3 Tile(this Transform hTransform)
    {
        if (hTransform == null)
            return Vector3.zero;

        return CGlobal_TilemapManager.GetTilePosition(hTransform.position);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 TileUp(this Transform hTransform)
    {
        if (hTransform == null)
            return Vector3.zero;

        return CGlobal_TilemapManager.GetNextTileInThisDirection(hTransform.position, TileDirection.Up);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 TileDown(this Transform hTransform)
    {
        if (hTransform == null)
            return Vector3.zero;

        return CGlobal_TilemapManager.GetNextTileInThisDirection(hTransform.position, TileDirection.Down);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 TileLeft(this Transform hTransform)
    {
        if (hTransform == null)
            return Vector3.zero;

        return CGlobal_TilemapManager.GetNextTileInThisDirection(hTransform.position, TileDirection.Left);
    }

    /// <summary>
    /// 
    /// </summary>
    public static Vector3 TileRight(this Transform hTransform)
    {
        if (hTransform == null)
            return Vector3.zero;

        return CGlobal_TilemapManager.GetNextTileInThisDirection(hTransform.position, TileDirection.Right);
    }
}
