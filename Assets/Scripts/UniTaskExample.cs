using Cysharp.Threading.Tasks;
using UnityEngine;

public class UniTaskExample : MonoBehaviour
{
    [SerializeField] private float waitSeconds = 1.5f;

    private async void Start()
    {
        Debug.Log("[UniTaskExample] 작업 시작");

        int result = await CalculateAfterDelayAsync(waitSeconds, this.GetCancellationTokenOnDestroy());

        Debug.Log($"[UniTaskExample] 결과: {result}");
    }

    private static async UniTask<int> CalculateAfterDelayAsync(float seconds, System.Threading.CancellationToken cancellationToken)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(seconds), cancellationToken: cancellationToken);
        return 42;
    }
}
