using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 받아온 좌표를 기준으로 타일의 위치 또는 정보를 받아오기 위한 매니저 클래스
/// </summary>
public class TileManager : Singleton<TileManager>
{
    [SerializeField] private GridLayout gridLayout;     //타일 좌표를 받을 Grid

    [SerializeField] private Tilemap groundTilemap;     //흙 타일을 까는 타일맵
    [SerializeField] private Tile groundTile;           //흙 타일

    public Vector3Int interactTile { get; private set; } = Vector3Int.zero;

    private Vector3Int[] dirs =                         //해당 타일의 상하좌우에 타일이 있는지 검사하기 위한 좌표 증감값
        {
            new Vector3Int(1, 0, 0),    //우
            new Vector3Int(-1, 0, 0),   //좌
            new Vector3Int(0, 1, 0),    //상
            new Vector3Int(0, -1, 0)    //하
        };

    [SerializeField] private Transform tileCursor;      //현재 선택할 수 있는 타일을 시각적으로 보여주는 커서

    public void GetTile(Vector3Int position)
    {
        //현재 보고있는 타일의 좌표를 갱신
        interactTile = position;

        //선택중인 타일을 시각적으로 보여주는 커서 위치 갱신
        tileCursor.position = position;

    }

    //괭이질을 하면 타일을 검사 후 해당 타일을 흙으로 변환
    public void GetTileToSoil()
    {
        if(isNearByTile(groundTilemap, interactTile))
        {
            //현재 타일 상하좌우에 땅이 있으면 실행
            groundTilemap.SetTile(interactTile, groundTile);
        }
    }


    /// <summary>
    /// 받아온 타일 좌표에 인접한 상하좌우 좌표에 타일이 있는지 검사 true → 모든 인접 좌표에 타일이 있음, false → 한 곳이라도 없을 경우
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool isNearByTile(Tilemap tilemap, Vector3Int pos)
    {
        foreach(var dir in dirs)
        {
            TileBase neighborTile = tilemap.GetTile(pos + dir);

            if (neighborTile == null)
                return false;
        }

        return true;
    }
}
