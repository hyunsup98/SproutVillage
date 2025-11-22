using System.Collections.Generic;
using UnityEngine;

public static class CoroutineManager
{
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>();
    private static readonly Dictionary<float, WaitForSecondsRealtime> _waitForSecondsRealtime = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSeconds waitForSeconds(float seconds)
    {
        //캐싱할 WaitForSeconds추가 => 인자로 받은 seconds가 _waitForSeconds에 없다면 추가해줌
        if (!_waitForSeconds.TryGetValue(seconds, out var _seconds))
        {
            _waitForSeconds.Add(seconds, _seconds = new WaitForSeconds(seconds));
        }

        return _seconds;
    }

    public static WaitForSecondsRealtime waitForSecondsRealtime(float seconds)
    {
        if (!_waitForSecondsRealtime.TryGetValue(seconds, out var _seconds))
        {
            _waitForSecondsRealtime.Add(seconds, _seconds = new WaitForSecondsRealtime(seconds));
        }

        return _seconds;
    }
}
