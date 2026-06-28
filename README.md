
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
목적: 
- [`TableSOGeneratorWindow`](Assets/0.Scripts/CSVParser/Editor/TableSOGeneratorWindow.cs)
  - Unity Editor 메뉴에서 CSV 경로와 SO 저장 경로를 지정하고, 파싱 및 SO 생성을 실행하는 에디터 툴 구현 클래스
  - EditorPrefs를 사용해 경로 저장, 
 
- [`TableSOGenerator`](Assets/0.Scripts/CSVParser/Editor/TableSOGenerator.cs)
  - CSV 테이블을 파싱해 데이터용 ScriptableObject 클래스와 Database 클래스를 자동 생성하고, CSV 데이터를 실제 SO 에셋으로 주입하는 핵심 생성 로직
  - 파싱 과정: `에디터 창에서 경로 지정` → `CSV 헤더를 읽어서 컬럼 정보로 변환` → `Scriptable Object 클래스 코드를 생성` → `실제 값이 들어간 Scriptable Object 생성`

- [`CsvClassData`](Assets/0.Scripts/CSVParser/Editor/CsvClassData.cs)
  - CSV 헤더, 컬럼 타입, 데이터 개수 등 파싱 결과를 담는 데이터 컨테이너

 - [`TableBase`](Assets/0.Scripts/CSVParser/Table/TableBase.cs) & [`TableDatabase`](Assets/0.Scripts/CSVParser/Table/TableDatabase.cs)
   - 데이터 파서 툴을 이용해 생성된 DataSO와 DatabaseSO가 공통으로 상속받는 기반 클래스
   - DataSO에서는 데이터의 고유 ID를 반환하도록 하는 추상 메서드 구현, DatabaseSO에서는 인덱서를 통해 ID 기준 조회가 가능하도록 구현

### 2. 동물 AI
목적: 



### 3. 길찾기 알고리즘
목적: 



### 4. 타일맵 관리 시스템
목적: 



### 5. 농장 시스템
목적: 



### 6. Json 파싱 기반 데이터 저장&로드
목적: 



























