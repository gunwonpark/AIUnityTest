# AIUnityTest
AI를 활용하여 이것저것 해볼려고하는 공간

## UniTask 예제
- `Assets/Scripts/UniTaskExample.cs`를 빈 GameObject에 붙이면, 시작 시 `waitSeconds` 만큼 기다린 뒤 결과(`42`)를 로그로 출력합니다.
- 오브젝트가 파괴되면 `GetCancellationTokenOnDestroy()`로 대기 작업이 자동 취소됩니다.
