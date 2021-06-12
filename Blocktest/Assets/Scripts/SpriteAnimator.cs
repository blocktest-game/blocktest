using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private int currentSprite;
    private SpriteRenderer spriteRenderer;
    private SpriteSheet spriteSheet;
    private float timePerFrame;
    private float lastFrameTime;
    private bool cycling;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (!cycling) { return; }
        if (!(Time.time - lastFrameTime >= timePerFrame)) { return; }
        lastFrameTime = Time.time;
        spriteRenderer.sprite = spriteSheet[++currentSprite % spriteSheet.Length];
    }

    public void SetAnimation(SpriteSheet newSheet, float newTime = 0.1f)
    {
        if (newSheet.Equals(spriteSheet)) { return; }
        spriteRenderer.sprite = newSheet[0];
        currentSprite = 0;
        spriteSheet = newSheet;
        cycling = newSheet.Length != 1;
        timePerFrame = newTime;
    }
}