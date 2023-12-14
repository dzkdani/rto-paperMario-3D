using UnityEngine;
using DG.Tweening;
using System;

public static class Helper
{
    public static Vector2 ToVector2 (this Vector3 move) => new Vector2(move.x, move.z);

    public static void FlipSprite (this Transform transform, Action onComplete = null) => transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.4f).From(transform.rotation.eulerAngles).SetEase(Ease.InQuad)
    .OnComplete(onComplete.Invoke);
    
}
