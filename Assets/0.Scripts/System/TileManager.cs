using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 받아온 좌표를 기준으로 타일의 위치 또는 정보를 받아오기 위한 매니저 클래스
/// </summary>
public class TileManager : Singleton<TileManager>
{
    [SerializeField] private GridLayout gridLayout;     //타일 좌표를 받을 Grid

    [field: SerializeField] public Tilemap groundTilemap { get; private set; }      //잔디 타일이 깔려있는 기본 타일맵, 괭이질을 할 수 있냐 등을 판단하기 위함
    [field: SerializeField] public Tilemap soilTilemap { get; private set; }        //씨앗을 심을 수 있는, 나무를 심을 수 있는 등의 타일
    [field: SerializeField] public Tilemap wetSoilTilemap { get; private set; }     //괭이질 된 땅에 물을 뿌릴 경우 젖은 흙으로 변경하기 위함
    [field: SerializeField] public Tilemap plantTilemap { get; private set; }       //작물을 심을 타일맵
    [field: SerializeField] public Tilemap foregroundTilemap { get; private set; }  //구조물 등이 설치될 타일맵
    [field: SerializeField] public RuleTile groundTile { get; private set; }        //흙 타일
    [field: SerializeField] public WetTile wetTile { get; private set; }            //젖은 타일을 관리하는 클래스

    public Vector3Int interactTile { get; private set; } = Vector3Int.zero; //현재 상호작용을 할 타일

    private Vector3Int[] dirs =                         //해당 타일의 상하좌우에 타일이 있는지 검사하기 위한 좌표 증감값
        {
            new Vector3Int(1, 0, 0),    //우
            new Vector3Int(-1, 0, 0),   //좌
            new Vector3Int(0, 1, 0),    //상
            new Vector3Int(0, -1, 0)    //하
        };

    [SerializeField] private Transform tileCursor;      //현재 선택할 수 있는 타일을 시각적으로 보여주는 커서

    //마우스 커서 위치를 기반으로 현재 상호작용 가능한 타일의 좌표를 가지고 있음, 어떤 타일맵이든 좌표로 바로 접근 가능
    public void ViewTile(Vector3Int position)
    {
        //현재 보고있는 타일의 좌표를 갱신
        interactTile = position;

        //선택중인 타일을 시각적으로 보여주는 커서 위치 갱신
        tileCursor.position = position;
    }

    public TileBase GetTile(Tilemap tilemap, Vector3Int tilePos)
    {
        TileBase tile = tilemap.GetTile(tilePos);
        return tile;
    }

    public bool HasTile(Tilemap tilemap, Vector3Int tilePos)
    {
        TileBase tile = tilemap.GetTile(tilePos);

        if (tile != null)
            return true;

        return false;
    }

    public bool HasTile(Tilemap tilemap, Vector3Int tilePos, params TileBase[] tiles)
    {
        TileBase tile = GetTile(tilemap, tilePos);

        foreach (var t in tiles)
        {
            if (tile == t)
                return true;
        }

        return false;
    }

    public void SetTile(Tilemap tilemap, Vector3Int tilePos, GameObject obj)
    {
        Vector3 worldPos = tilePos + tilemap.tileAnchor;
        obj.transform.position = worldPos;
        obj.transform.SetParent(tilemap.transform);
    }

    //괭이질을 하면 타일을 검사 후 해당 타일을 흙으로 변환 또는 제거
    public void SetTileToSoil()
    {
        TileBase tile = soilTilemap.GetTile(interactTile);

        //상호작용할 타일에 아무것도 없고, 근처 상하좌우에 타일이 있을 경우
        if (tile == null)
        {
            if(isNearByTile(groundTilemap, interactTile))
            {
                //흙 깔기
                soilTilemap.SetTile(interactTile, groundTile);
            }
        }
        else //해당 타일에 어떤 타일이 있을 경우
        {
            //그 타일이 흙일 경우
            if(tile == groundTile)
            {
                //흙 타일 지우기
                soilTilemap.SetTile(interactTile, null);
            }
        }

        tile = wetSoilTilemap.GetTile(interactTile);

        //젖은 타일을 별개로 두기 때문에 혹시 있다면 같이 제거
        if(tile != null && tile == groundTile)
        {
            wetSoilTilemap.SetTile(interactTile, null);
        }
    }

    //물을 준 타일에 흙이 있을 경우 wetSoilTilemap 타일맵에 젖은 타일을 추가
    public void SetTileToWetSoil()
    {
        TileBase tile = soilTilemap.GetTile(interactTile);

        //물 주는 곳에 흙 타일이 있을 경우
        if(tile != null && tile == groundTile)
        {
            wetTile.AddWetTile(interactTile);
            wetSoilTilemap.SetTile(interactTile, groundTile);
        }
    }

    //젖어있는 시간이 끝나서 젖은 타일을 지우는 메서드
    public void RemoveWetTile(Vector3Int tilePos)
    {
        TileBase tile = wetSoilTilemap.GetTile(tilePos);

        if(tile != null && tile == groundTile)
        {
            wetSoilTilemap.SetTile(tilePos, null);
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
