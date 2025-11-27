using System;

//최소 힙 트리를 이용한 우선순위 큐 구현
//힙 트리는 부모와 자식 간의 비교가 보장되지 같은 레벨의 노드끼리는 보장되지 않음
public class PriorityQueue<T> where T : IComparable<T>
{
    //값이 들어갈 배열
    T[] data;

    public int Count { get; private set; }
    public int Capacity { get; private set; }

    #region 생성자
    public PriorityQueue(int capacity = 1)
    {
        Count = 0;
        Capacity = capacity;
        data = new T[Capacity];
    }
    #endregion

    public T Value(int index)
    {
        return data[index];
    }

    public void Enqueue(T value)
    {
        //배열이 꽉 찼으면 확장
        if (Count >= Capacity)
            Expand();

        //데이터 추가
        data[Count] = value;
        Count++;

        int now = Count - 1;
        while(now > 0)
        {
            //부모 노드의 인덱스
            int parent = (now - 1) / 2;

            //CompareTo() : 현재의 객체가 대상 객체보다 작으면 0보다 작은 값을, 같으면 0을, 크면 0보다 큰 값을 반환.
            //부모 노드의 값이 더 작으면 정지
            if (data[now].CompareTo(data[parent]) > 0)
                break;

            //현재 노드와 부모 노드의 값 바꾸기
            T temp = data[now];
            data[now] = data[parent];
            data[parent] = temp;

            //현재 위치 갱신 → 부모 노드 위치로 감
            now = parent;
        }
    }

    public T Dequeue()
    {
        //배열에 값이 없으면 예외 발생
        if (Count == 0)
            throw new IndexOutOfRangeException();

        T result = data[0];             //반환할 루트 노드의 값 받음
        data[0] = data[Count - 1];      //빈자리가 된 루트 노드에는 마지막 노드의 값으로 대체 → 나중에 힙 재정렬 하기 위함
        data[Count - 1] = default(T);   //마지막 노드에는 T 타입의 기본값을 넣어줌
        Count--;                        //배열 크기 줄임 → 마지막 노드는 더 이상 배열에 포함되지 않음

        //재정렬
        //루트부터 시작하여 자식 노드 중 큰 쪽과 비교, 현재 노드가 더 크다면 교환
        int now = 0;
        while(now < Count)
        {
            int left = (now * 2) + 1;   //현재 노드의 왼쪽 자식 노드 인덱스
            int right = (now * 2) + 2;  //현재 노드의 오른쪽 자식 노드 인덱스

            int next = now;

            //왼쪽 노드가 존재하고 값이 더 작으면 next 갱신
            if (left < Count && data[next].CompareTo(data[left]) > 0)
                next = left;

            //오른쪽 노드가 존재하고 값이 더 작으면 next 갱신
            if (right < Count && data[next].CompareTo(data[right]) > 0)
                next = right;

            //갱신되지 않았다면 루프 종료
            if (next == now)
                break;

            //부모 노드와 자식 노드의 값 바꾸기
            T temp = data[now];
            data[now] = data[next];
            data[next] = temp;

            //현재 위치 갱신
            now = next;
        }

        return result;
    }

    public T Peek()
    {
        //배열에 값이 없으면 예외 발생
        if (Count == 0)
            throw new IndexOutOfRangeException();

        return data[0];
    }

    public bool Any() => Count != 0;

    //배열 크기 확장
    private void Expand()
    {
        T[] newData = new T[Capacity * 2];
        for(int i = 0; i < Count; i++)
        {
            newData[i] = data[i];
        }

        data = newData;
        Capacity *= 2;
    }
}
