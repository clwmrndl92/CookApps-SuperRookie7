# CookApps-SuperRookie7
CookApps 슈퍼루키7기 지원 과제 전형으로 제출한 짧은 방치형 RPG입니다.   
제작기간 : 24.05.13 ~ 24.05.20 (7일간)   
apk다운로드 : https://drive.google.com/file/d/18yVbGOgJryOy100bzunV9tquEeZpjbf7/view?usp=sharing

## 구현 사항 개요
2개의 스테이지가 있는 간단한 방치형 RPG 게임구현

### 기본 구현 사항
 +	4명의 캐릭터와 자리배치
 	+ 카메라는 1번슬롯을 추적   
 +	캐릭터 사망 후 일정시간 지나면 부활
 	+ 모든 캐릭터 사망시 스테이지 재시작
 +	몬스터는 일정주기로 캐릭터 근처에서 스폰
 +	State패턴을 사용하여 상황에 맞는 행동 및 애니메이션 수행
 	+ 캐릭터와 몬스터는 DetectRange안에 있을 때 서로 추적함
 	+ 캐릭터와 몬스터의 일반공격 및 특수 능력 구현

  ### 추가 항목
 +	몬스터 처치시 exp를 획득하고 이를 통해 플레이어 레벨업 (캐릭터별 아님)
 	+ 플레이어 레벨업 시 캐릭터 능력치 상승
 	+ 캐릭터 하단 체력 게이지 UI
 	+ 캐릭터별 스킬 쿨타임 표시
 	+ 플레이어 exp, 레벨 표시
 +	보스몬스터 처치를 통한 스테이지 등반
 	+ 몬스터 N 마리 처치 시 보스 몬스터 등장
 	+ 보스 몬스터 처치 시 다음 스테이지로 이동
 	+ 스테이지 이동시 몬스터 능력치 강화
 	+ 2개의 스테이지와 2가지 종류의 몬스터 및 보스몬스터 구현
 +	재화로 캐릭터 강화시스템 (골드, 루비 두가지 재화)
 	+ 일반 몬스터 처치시 골드만, 보스몬스터 처치시 골드, 루비 획득
 +	게임 플레이 배속 기능 (1~3배속)
 +	안드로이드 빌드로 플레이 가능

## 구현 사항 상세

### 게임 세팅
게임 세팅은 Asset/SO/Installer/GameSettingInstallerObject에서 세팅 가능   
![image](https://github.com/clwmrndl92/CookApps-SuperRookie7/assets/50985650/f5bfb0c2-b1c4-42a5-ab56-ba0b5bda6c88)

 +	젠젝트 팩토리 바인딩할 프리팹
 +	스테이지 추가
 +	등장하는 몬스터
 +	캐릭터 능력치 기본 값 설정

### 게임 진행
게임 시작 -> 스테이지 진행 -> 마지막 스테이지 클리어 시 게임 클리어
 +	모든 캐릭터 사망(게임 오버) 현재 스테이지 재시작
 +	현재 두개의 스테이지가 구현되어 있음, 스테이지는 ScriptableObject로 설정하여 쉽게 추가 가능함
![image](https://github.com/clwmrndl92/CookApps-SuperRookie7/assets/50985650/4a66bd0e-bc7b-457d-8ce4-ad8b3a87a779)


### 캐릭터와 몬스터
캐릭터와 몬스터는 Unit을 상속받아 스테이트 패턴을 사용하여 상황에 맞는 적절한 행동과 애니메이션을 취함

 +	캐릭터
 	+ 캐릭터는 감지 범위 내 몬스터를 추적하여 공격함, 없을 시 자기 자리로 돌아가서 가만히 서있음
 	+ 캐릭터는 일반 공격과 특수 스킬을 가지고 있고 자동으로 순환하며 사용함
 	+ 캐릭터는 플레이어가 레벨업 할 때마다 HP와 공격력이 상승함

 +	몬스터
 	+ 몬스터는 감지 범위 내 캐릭터를 추적하여 공격함, 없을 시 가만히 서있음
 	+ 몬스터 처치 시 경험치와 재화를 획득함
 	+ 몬스터는 스테이지가 올라갈수록 N*10%씩 강해짐
 	+ 몬스터 Scriptable Object를 만들어 쉽게 추가 가능함
  ![image](https://github.com/clwmrndl92/CookApps-SuperRookie7/assets/50985650/679d23ec-a052-4ec9-90f3-5932880ca3e3)

 

 +	보스몬스터
 	+ 보스몬스터는 기본적으로 몬스터와 같음
 	+ 보스몬스터 처치 시 경험치와 골드, 루비를 획득함
 	  + 루비는 골드와 다른 업그레이드에 사용
 	+ 보스몬스터는 캐릭터처럼 특수 공격을 사용함
 	  + 킹고블린 : 광역 공격
 	  + 킹플라잉아이 : 한명 대상 즉사기
 	+ 일반 몬스터 일정 수 이상 처치 시 보스몬스터가 등장함
 	  + 보스몬스터 등장 시 보스몬스터 이름과 HP바가 뜸
 	  + 보스몬스터 처치 시 다음 스테이지로 넘어감


### 강화시스템 구현
 +	몬스터를 처치하여 얻은 골드와 루비로 캐릭터의 체력, 공격력, 스킬계수(루비)를 업그레이드 시킬 수 있음

### 기타
 +	게임 진행속도 배속 기능
 +	UI : UnitRx 사용
 	+ 캐릭터 체력, 스킬 쿨타임 표시
 	+ 보스 소환까지 남은 몬스터 처치 수 표시
