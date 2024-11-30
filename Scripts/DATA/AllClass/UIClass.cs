using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UIClassScale
{

   public float fanimationSpeed = 2;//动画播放速度

    public UIClassScale()
    {

    }



    /// <summary>
    /// 根据传入的曲线变大变小
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <returns></returns>
   public  IEnumerator fnScaleChange_BeginAtOne(GameObject _gameObject,AnimationCurve _curve)
    {
        float timer = 0;
        while (timer <= 1)
        {
            float fTmp = GetAnimationCurveFloat(_curve, timer);
            ChangeScale(_gameObject,fTmp);
            timer += Time.deltaTime * fanimationSpeed;
            yield return null;
        }
    }

    /// <summary>
    /// 获取动画曲线的值
    /// </summary>
    /// <param name="_tmpCurve">曲线动画</param>
    /// <param name="timer">时间</param>
    /// <returns></returns>
    float GetAnimationCurveFloat(AnimationCurve _tmpCurve,float timer)
    {
        float fValue = _tmpCurve.Evaluate(timer);
        return fValue;
    }



    /// <summary>
    /// 改变物体大小
    /// </summary>
    /// <param name="_gameObject">哪个物体</param>
    /// <param name="_scale">缩放的比例</param>
    void ChangeScale(GameObject _gameObject, float _scale)
    {
        _gameObject.transform.localScale = Vector3.one * _scale;
    }
}


public class UIClassPos
{
  
    /// <summary>
    /// 平滑移动
    /// </summary>
    /// <param name="rectTransform">移动的对象</param>
    /// <param name="curPos">起点位置</param>
    /// <param name="targetPos">终点位置</param>
    /// <param name="animationCurve">速度曲线</param>
    /// <param name="duration">动画时间</param>
    /// <returns></returns>
    public  IEnumerator SmoothMoveRectTransform(RectTransform rectTransform, Vector2 curPos, Vector2 targetPos, AnimationCurve animationCurve, float duration)
    {
        float startTime = Time.time;
        Vector2 startPosition = curPos;
        Vector2 startAnchoredPosition = rectTransform.anchoredPosition;

        // 计算从锚点到起始位置的偏移量
        Vector2 startOffsetFromPivot = startPosition - (rectTransform.pivot * rectTransform.sizeDelta);

        // 计算从锚点到目标位置的偏移量
        Vector2 targetOffsetFromPivot = targetPos - (rectTransform.pivot * rectTransform.sizeDelta);

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            t = Mathf.Clamp(t, 0.0f, 1.0f);

            // 获取曲线在 t 处的值，作为速度因子（注意：曲线应该被设计为从 0 到 1）
            float speedFactor = animationCurve.Evaluate(t);

            // 使用 LerpUnclamped 来避免在 t=1 时突然跳跃（通常不需要，除非曲线不是标准的 0-1 曲线）
            // 但由于我们使用了 speedFactor，它可能会改变插值的速度，所以这里保留 LerpUnclamped
            Vector2 newOffsetFromPivot = Vector2.LerpUnclamped(startOffsetFromPivot, targetOffsetFromPivot, t * speedFactor);

            // 更新 RectTransform 的锚点位置（偏移量）
            rectTransform.anchoredPosition = newOffsetFromPivot + rectTransform.pivot * rectTransform.sizeDelta;


            yield return null;
        }


        rectTransform.anchoredPosition = targetOffsetFromPivot + rectTransform.pivot * rectTransform.sizeDelta;

        
    }
}
