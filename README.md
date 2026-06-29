
## 프로젝트 소개

<div align=center>
<img width="648" height="384" alt="image" src="https://github.com/hyunsup98/SproutVillage/blob/main/Assets/MainProfile.png" />

<br>
'Sprout Village'는 PC 플랫폼의 탑뷰 2D 힐링 농사 게임입니다.
<br>
</div>


## 사용 기술 스택

- Engine & Language : `Unity 2D`, `C#`
- Graphics & Pipeline : `URP`
- Library & Plugins : `DOTWeen`
- Version Control : `Github`

<br><br>

## 담당 기능
💡 **클래스명을 클릭하면 해당 소스 코드로 이동합니다.**

### 1. 플레이어 컨트롤러
목적: 플레이어 조작 입력과 행동 상태를 통합 관리하고, 이동/상호작용/애니메이션 흐름을 연결, 코드 재사용성을 높이기 위한 계층적 구조 상태 패턴 설계
- [`PlayerController`](Assets/0.Scripts/Player/PlayerController.cs)
  - Input System 기반으로 이동, 달리기, 대쉬 등의 키 입력 처리
  - 여러 상태 클래스에서 공통으로 수행되는 함수 구현
 
- [`PlayerState 폴더`](Assets/0.Scripts/Player/PlayerState)
  - `PlayerMovementState.cs`, `PlayerIdleState.cs`, `PlayerToolState.cs`: (이동/달리기/대쉬) 또는 (괭이/물뿌리개/도끼) 등의 상태가 공통적으로 행하는 기능을 가지는 부모 상태 클래스
  - `PlayerMoveState.cs`, `PlayerRunState.cs`, `PlayerDashState.cs`: PlayerMovementState 클래스를 상속받는 이동 관련 상태
  - `PlayerWateringPotState.cs`, `PlayerHoeState.cs`, `PlayerAxeState.cs`: PlayerToolState 클래스를 상속받는 도구 사용 관련 상태
 

### 2. 동물 AI
목적: 동물의 생산 활동, 이동 등의 AI 구현, 추후 새로운 행동 추가 등의 확장성에 열려있는 구조로 설계
- [`AnimalAI`](Assets/0.Scripts/Animal/AnimalAI.cs)
  - 행동 트리(Behaviour Tree)를 사용해 동물 AI를 설계, 추후 추가될 수 있는 새로운 행동에 확장성있게 대응하기 위함

- [`BehaviorTreeRunner`](Assets/0.Scripts/DesignPattern/BehaviorTree/BehaviorTreeRunner.cs)
  - 루트 노드를 실행하는 행동 트리(Behaviour Tree) 구동 클래스

- [`SelectorNode`](Assets/0.Scripts/DesignPattern/BehaviorTree/SelectorNode.cs), [`SequenceNode`](Assets/0.Scripts/DesignPattern/BehaviorTree/SequenceNode.cs), [`ActionNode`](Assets/0.Scripts/DesignPattern/BehaviorTree/ActionNode.cs)
  - Selector, Sequence, Action 노드를 직접 구현하여 동물 AI 행동 흐름을 구성
 

### 3. 길찾기 알고리즘
목적: 타일맵 기반 장애물 정보를 활용해 동물이 목표 지점까지 이동할 경로를 계산
- [`AStar`](Assets/0.Scripts/DesignPattern/AStar/AStar.cs)
  - A* 알고리즘을 활용해 시작 위치부터 목표 위치까지의 최단 경로를 계산
  - 상하좌우 및 대각선 8방향 탐색을 지원하고, 이동 비용과 휴리스틱 값을 기반으로 최단 경로를 탐색
  - `TileManager`의 foreground 타일맵 정보를 활용해 이동 불가능한 타일을 경로에서 제외

- [`PriorityQueue`](Assets/0.Scripts/DesignPattern/AStar/PriorityQueue.cs)
  - A* 탐색에서 F 비용이 낮은 노드를 우선 처리하기 위한 최소 힙 트리 기반 우선순위 큐 구현
  - 다음 탐색 노드 선택 비용을 O(n)에서 O(log n)으로 개선


### 4. 타일맵 관리 시스템
목적: 플레이어 상호작용 타일, 농장 타일, 장애물 타일 등 여러 타일맵 정보를 중앙에서 관리
- [`TileManager`](Assets/0.Scripts/System/TileManager.cs)
  - Ground, Soil, WetSoil, Plant, Foreground Tilemap을 관리하는 타일 시스템 중심 클래스
  - 플레이어가 현재 바라보는 상호작용 타일을 갱신하고, 타일 커서 위치를 동기화
  - 괭이질, 물 주기, 작물 배치, 장애물 판정 등 여러 시스템에서 공통으로 사용하는 타일 조회/변경 기능을 제공

- [`WetTile`](Assets/0.Scripts/Tile/WetTile.cs)
  - 물을 준 타일의 지속 시간을 딕셔너리로 관리하고, 시간이 지나면 젖은 타일을 자동 제거
  - 작물 성장 조건과 연결되는 젖은 흙 상태를 코루틴 기반으로 갱신


### 5. 농장 시스템
목적: 씨앗 심기, 작물 성장, 수확, 토양 상태 변화를 타일맵 기반으로 처리
- [`Seed`](Assets/0.Scripts/Items/Seed.cs)
  - 사용 가능한 아이템 인터페이스 `IUsable`를 구현하여 선택한 타일에 작물을 심는 기능을 담당
  - 흙 타일 여부를 검사한 뒤 작물을 오브젝트 풀에서 가져와 Plant Tilemap 위치에 배치

- [`Plant`](Assets/0.Scripts/Plants/Plant.cs)
  - 작물의 성장 단계, 성장 가능 조건, 수확 가능 상태를 관리하는 작물 기반 클래스
  - 젖은 흙 타일 위에 있을 때만 성장 시간이 누적되도록 처리
  - 성장이 완료되면 플레이어 상호작용을 통해 수확 아이템을 인벤토리에 추가

- [`Hoe`](Assets/0.Scripts/Items/Tools/Hoe.cs) & [`WateringPot`](Assets/0.Scripts/Items/Tools/WateringPot.cs)
  - 괭이와 물뿌리개 도구의 실제 효과를 타일맵 시스템과 연결
  - 괭이는 흙 타일 생성/제거, 물뿌리개는 젖은 흙 타일 생성을 담당


### 6. 데이터 저장&로드 및 인벤토리 기능
목적: 인벤토리와 골드 데이터를 Json 기반으로 저장하고, 게임 시작 후 다시 복원
- [`DataManager`](Assets/0.Scripts/System/DataManager.cs)
  - 인벤토리 슬롯 정보와 플레이어 골드를 `SaveData` 객체로 변환한 뒤 Json으로 직렬화하여 저장
  - 저장 파일을 UTF-8 바이트 배열과 Base64 문자열로 인코딩하여 로컬 파일에 기록
  - 로드 시 저장된 아이템 ID를 기준으로 아이템 데이터를 찾아 인벤토리 슬롯과 골드 값을 복원

- [`Inventory`](Assets/0.Scripts/UI/Inventory/Inventory.cs)
  - 아이템 추가 시 같은 아이템 스택 여부를 먼저 검사하고, 남은 수량은 빈 슬롯에 배치
  - 저장/로드 시스템에서 복원된 아이템이 들어갈 실제 인벤토리 데이터 구조를 제공

- [`Slot`](Assets/0.Scripts/UI/Inventory/Slot.cs)
  - 슬롯 내 아이템, 개수, UI 갱신, 드래그 앤 드롭 처리를 담당하는 인벤토리 기본 슬롯 클래스


### 7. 오브젝트 풀링 시스템

목적: 아이템과 작물처럼 반복 생성/제거되는 오브젝트를 재사용하여 런타임 생성 비용을 줄임

- [`ObjectPool`](Assets/0.Scripts/DesignPattern/ObjectPool/ObjectPool.cs)
  - 제네릭 기반 오브젝트 풀을 구현하여 단일 타입 및 여러 타입의 오브젝트 재사용을 지원
  - 아이템, 작물 등 이름별 Queue를 통해 여러 프리팹 타입을 구분 관리

- [`ItemPool`](Assets/0.Scripts/DesignPattern/ObjectPool/ItemPool.cs) & [`PlantPool`](Assets/0.Scripts/DesignPattern/ObjectPool/PlantPool.cs)
  - 아이템 드랍, 동물 생산물, 작물 생성/수확 과정에서 사용되는 전용 오브젝트 풀 클래스























