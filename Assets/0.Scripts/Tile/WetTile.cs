using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WetTile : MonoBehaviour
{
    [SerializeField] private int wetTime;     //땅이 젖어있는 시간(초)

    //젖어있는 타일들을 관리할 딕셔너리, key → 젖은 타일의 x, y 좌표 value → 남은 시간
    private Dictionary<Vector3Int, int> dicWetTiles = new Dictionary<Vector3Int, int>();

    private Coroutine wetTimerCoroutine;

    //젖은 타일 추가
    public void AddWetTile(Vector3Int tilePos)
    {
        //이미 젖어있는 타일에 물을 또 주면 시간만 새로 갱신해줌
        if(dicWetTiles.ContainsKey(tilePos))
        {
            dicWetTiles[tilePos] = wetTime;
        }
        else //새로운 타일이면 딕셔너리에 추가
        {
            dicWetTiles.Add(tilePos, wetTime);
        }

        //코루틴이 돌지 않을때만 실행
        if (wetTimerCoroutine == null)
            wetTimerCoroutine = StartCoroutine(WetTileManage());
    }

    //젖은 타일 관리 코루틴
    private IEnumerator WetTileManage()
    {
        //젖어있는 타일이 있을 경우 계속 반복
        while(dicWetTiles.Count > 0)
        {
            foreach(var key in dicWetTiles.Keys.ToList())
            {
                //젖어있는 타일의 wetTime을 1초 감소
                dicWetTiles[key] -= 1;

                if (dicWetTiles[key] <= 0)
                {
                    TileManager.Instance.RemoveWetTile(key);
                    dicWetTiles.Remove(key);
                }
            }

            //1초 마다 반복
            yield return CoroutineManager.waitForSeconds(1f);
        }

        wetTimerCoroutine = null;
        yield break;
    }

    private void OnDisable()
    {
        if (wetTimerCoroutine != null)
            StopCoroutine(wetTimerCoroutine);
    }
}
