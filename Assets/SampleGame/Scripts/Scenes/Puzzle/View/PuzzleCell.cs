using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures.TransformGestures;
using System;
using System.Linq;
using UniRx;
using DG.Tweening;

public class PuzzleCellSwipeMessage
{
    public static PuzzleCellSwipeMessage Default = new PuzzleCellSwipeMessage();
    public int Index { get; set; }
    public SwipeDirection Direction { get; set; }
}

public class PuzzleCell : MonoBehaviour
{
    [System.Serializable]
    public class Parameter
    {
        public PuzzleCellType cellType;
        public PuzzleUnitType unitType;
        public Texture texture;
    }

    [SerializeField] TransformGesture transformGesture;
    [SerializeField] RawImage image;
    [SerializeField] Parameter[] parameters;
    [SerializeField] Slider hpSlider;

    public float longPressTime = 2F;
    public float swipeRecognizeLength = 1F;
    public float elapsedTime;
    public bool isStarted;
    public Vector3 move;
    public SwipeDirection swipeDirection;
    PuzzleUnitModel unitModel;
    public int Index { get; set; }

    void OnEnable()
    {
        transformGesture.TransformStarted += OnTransformStarted;
        transformGesture.Transformed += OnTransformed;
        transformGesture.TransformCompleted += OnTransformCompleted;
    }

    void OnDisable()
    {
        transformGesture.TransformStarted -= OnTransformStarted;
        transformGesture.Transformed -= OnTransformed;
        transformGesture.TransformCompleted -= OnTransformCompleted;
    }

    //TODO: long press で詳細
    // void Update()
    // {
    //     if (!isStarted) return;
    //     elapsedTime += Time.deltaTime;
    //     if (longPressTime <= elapsedTime)
    //     {
    //         isStarted = false;
    //         elapsedTime = 0F;
    //         Debug.Log($"LongPresses.");
    //         return;
    //     }
    // }

    public void Setup(int index, PuzzleCellType cellType, PuzzleUnitType unitType, PuzzleUnitModel unitModel)
    {
        this.Index = index;
        this.unitModel = unitModel;
        hpSlider.gameObject.SetActive(cellType == PuzzleCellType.EnemyCell);
        hpSlider.value = 1F;

        var parameter = parameters.FirstOrDefault(x => x.unitType == unitType && x.cellType == cellType);
        if (parameter != null)
        {
            image.texture = parameter.texture;
        }
        InitializeScale();
    }

    public void SetIndex(int index)
    {
        this.Index = index;
    }

    public void PlayDamage()
    {
        var toValue = (float)unitModel.hp / unitModel.maxHp;
        hpSlider.DOValue(toValue, 0.3F);
    }

    public void PlayRemove()
    {
        transform.DOScale(Vector3.zero, 0.3F);
    }

    void InitializeScale()
    {
        transform.localScale = Vector3.one;
    }

    void OnTransformStarted(object sender, EventArgs e)
    {
        elapsedTime = 0F;
        isStarted = true;
        move = Vector3.zero;
        swipeDirection = SwipeDirection.None;
    }

    void OnTransformed(object sender, EventArgs e)
    {
        var g = transformGesture;
        move += g.LocalDeltaPosition;
        CheckSwipe();
    }

    void OnTransformCompleted(object sender, EventArgs e)
    {
        CheckSwipe();
    }

    void CheckSwipe()
    {
        if (swipeRecognizeLength <= Math.Abs(move.x))
        {
            isStarted = false;
            elapsedTime = 0F;
            if (move.x >= 0F)
            {
                swipeDirection = SwipeDirection.HorizontalPlus;
            }
            else
            {
                swipeDirection = SwipeDirection.HorizontalMinus;
            }
        }
        else if (swipeRecognizeLength <= Math.Abs(move.y))
        {
            isStarted = false;
            elapsedTime = 0F;
            if (move.y >= 0F)
            {
                swipeDirection = SwipeDirection.VerticalPlus;
            }
            else
            {
                swipeDirection = SwipeDirection.VerticalMinus;
            }
        }

        if (swipeDirection != SwipeDirection.None)
        {
            transformGesture.Cancel();
            var message = PuzzleCellSwipeMessage.Default;
            message.Index = Index;
            message.Direction = swipeDirection;
            MessageBroker.Default.Publish(message);
        }
    }
}
