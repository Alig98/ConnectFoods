using UnityEngine;

public class GridCreator : SingletonBase<GridCreator>
{
    //Fields
    [SerializeField] private Tile m_TilePrefab;
    [SerializeField] private Transform m_TileParent;

    //Public Methods
    public void CreateGrid(Vector3 gridStartPoint,Vector3 gridEndPoint,int rowCount,int columnCount)
    {
        var tileSizeForRow = Mathf.Abs((gridStartPoint.x - gridEndPoint.x)) / (rowCount-1);
        var tileSizeForColumn = Mathf.Abs((gridStartPoint.y - gridEndPoint.y)) / (columnCount-1);

        var tileSize = Mathf.Min(tileSizeForColumn,tileSizeForRow);
        tileSize = Mathf.Clamp01(tileSize);

        for (int j = 0; j < columnCount; j++)
        {
            for (int i = 0; i < rowCount; i++)
            {
                var tile = Instantiate(m_TilePrefab,m_TileParent);
                
                TileManager.Instance.AddToAllTiles(tile,i,j,tileSize);
            }
        }

        if (gridStartPoint.x+(rowCount-1 * tileSize) < gridEndPoint.x)
        {
            var dif = (gridEndPoint.x - (gridStartPoint.x + ((rowCount-1) * tileSize)))*.5f;

            var pos = m_TileParent.position;
            pos.x += dif;

            m_TileParent.position = pos;
        }
    }
}