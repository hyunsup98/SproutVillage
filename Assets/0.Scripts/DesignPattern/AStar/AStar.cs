using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IComparable<AStarNode>
{
    public Vector2Int index;    //해당 좌표 인덱스 → (x, y)
    public float G;             //출발지부터 현재 노드까지의 비용
    public float H;             //휴리스틱 → 현재 노드로부터 목적지까지의 예상 비용
    public float F => G + H;             //F = G + H → 출발지부터 현재 노드까지의 비용 + 현재 노드로부터 목적지까지의 예상 비용
    public AStarNode parent;    //부모 노드

    public AStarNode(Vector2Int index, float g, float h, AStarNode parent = null)
    {
        this.index = index;
        G = g;
        H = h;
        this.parent = parent;
    }

    public int CompareTo(AStarNode other)
    {
        int result = F.CompareTo(other.F);

        if (result != 0)
            return result;

        result = index.x.CompareTo(other.index.x);

        if (result != 0)
            return result;

        return index.y.CompareTo(other.index.y);
    }
}

public class AStar
{
    private static float Heuristic(Vector2Int a, Vector2Int b) => Vector2Int.Distance(a, b);

    //8방향 정보 배열
    private Vector2Int[] neighborsDir =
    {
        new Vector2Int(1, 0),   //오른쪽
        new Vector2Int(-1, 0),  //왼쪽
        new Vector2Int(0, 1),   //위
        new Vector2Int(0, -1),  //아래
        new Vector2Int(1, 1),   //오른쪽 위
        new Vector2Int(-1, 1),  //왼쪽 위
        new Vector2Int(1, -1),  //오른쪽 아래
        new Vector2Int(-1, -1)  //왼쪽 아래
    };

    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        //출발점, 도착점 설정
        Vector2Int startIndex = Vector2Int.FloorToInt(new Vector2(startPos.x, startPos.y));
        Vector2Int endIndex = Vector2Int.FloorToInt(new Vector2(endPos.x, endPos.y));

        if (!IsValidIndex(startIndex, endIndex))
            return null;

        //우선순위큐 생성
        PriorityQueue<AStarNode> queue = new PriorityQueue<AStarNode>();

        //방문여부를 저장할 HashSet 타입 생성
        //HashSet 사용 이유 → 중복되지 않은 요소들을 관리하는데 최적화 되어있음
        //Vector2Int를 통해 판단해야하는 만큼 bool 배열 등보다 관리하기 용이하다고 판단
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(new AStarNode(startIndex, 0, Heuristic(startIndex, endIndex)));

        while(queue.Count > 0)
        {
            //가장 작은 F를 가진 노드 가져오기
            AStarNode currentNode = queue.Dequeue();

            //현재 노드가 도착지라면
            if(currentNode.index == endIndex)
            {
                //이동 경로 반환
                return GetPath(currentNode, startPos, endPos);
            }

            //방문 처리
            visited.Add(currentNode.index);

            //이동할 수 있는 인접한 노드들 체크
            for(int i = 0; i < neighborsDir.Length; i++)
            {
                //인접한 타일 좌표 구하기 → 현재 노드 좌표 + neighborsDir의 좌표 (ex, (1, 0) = 오른쪽)
                Vector2Int index = new Vector2Int(currentNode.index.x + neighborsDir[i].x,
                    currentNode.index.y + neighborsDir[i].y);

                if (!visited.Contains(index) && IsValidIndex(currentNode.index, index))
                {
                    if(MathF.Abs(neighborsDir[i].x) == 1 && MathF.Abs(neighborsDir[i].y) == 1)
                    {
                        //대각선일 때
                        float g = currentNode.G + 1.4f;
                        float h = Heuristic(index, endIndex);
                        queue.Enqueue(new AStarNode(index, g, h, currentNode));
                    }
                    else
                    {
                        //상하좌우일 때
                        float g = currentNode.G + 1f;
                        float h = Heuristic(index, endIndex);
                        queue.Enqueue(new AStarNode(index, g, h, currentNode));
                    }
                }
            }
        }

        return null;
    }

    //도착지에 도달하면 이동 경로를 반환
    public List<Vector3> GetPath(AStarNode endNode, Vector3 start, Vector3 end)
    {
        //최종 이동 경로
        List<Vector3> path = new List<Vector3>();
        AStarNode node = endNode;

        //node가 null이 아닐 때
        while(node != null)
        {
            //해당 경로들을 리스트에 추가
            Vector3 pos = new Vector3(node.index.x, node.index.y, 0f);
            path.Add(pos);

            //노드가 출발 지점이 아니라면 부모 노드를 다시 넣어줌
            node = node.parent;
        }
        //이동 경로의 순서를 정렬 시작지점 → 출발지점 경로
        path.Reverse();

        //시작 지점과 도착 지점을 정확한 좌표값으로 설정
        path[0] = start;
        path[path.Count - 1] = end;
        return path;
    }

    //이동할 수 있는 타일인지 체크 true → 이동 가능한 타일 false → 이동 불가능한 타일
    private bool IsValidIndex(Vector2Int startIndex, Vector2Int neighborIndex)
    {
        //시작 지점과 너무 먼 범위의 타일까지 검사할 경우 제외
        if (neighborIndex.x < -20 || neighborIndex.y < -20 || neighborIndex.x > 20 || neighborIndex.y > 20)
            return false;
        
        //해당 좌표 타일맵에 이동 불가능한 타일이 있을 경우
        Vector3Int tilePos = new Vector3Int(neighborIndex.x, neighborIndex.y, 0);
        return !TileManager.Instance.GetTile(TileManager.Instance.foregroundTilemap, tilePos);
    }
}