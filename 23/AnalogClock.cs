using System;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    public Transform hourHand;    // 시침에 대한 참조
    public Transform minuteHand;  // 분침에 대한 참조

    private void Update()
    {
        // 현재 시스템 시간 가져오기
        DateTime now = DateTime.Now;

        // 시간과 분에 따라 시침과 분침 회전 계산
        float hourDegrees = (now.Hour % 12 + now.Minute / 60.0f) * 30.0f;  // 한 시간에 30도
        float minuteDegrees = now.Minute * 6.0f;  // 한 분에 6도

        // 시침과 분침에 회전 적용
        hourHand.localRotation = Quaternion.Euler(0, 0, -hourDegrees);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteDegrees);
    }
}
