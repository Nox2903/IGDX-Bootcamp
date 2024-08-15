using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using checkPointsManager.runtime;

public class FadingUI : MonoBehaviour
{
    public static FadingUI instance;
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;
    public TMP_Text textDesc;
    public Player_Checkpoint player_Checkpoint;
    public GameObject player;

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }
    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
    private void Fade(float endValue, float duration, TweenCallback OnEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }
        fadeTween = canvasGroup.DOFade(endValue, duration);
        fadeTween.onComplete += OnEnd;
    }
    private IEnumerator FadingIn()
    {
        yield return new WaitForSeconds(0f);
        FadeIn(0.5f);
    }
    private IEnumerator FadingOut()
    {
        yield return new WaitForSeconds(1f);
        FadeOut(1f);
    }

    public IEnumerator TestFadeIn()
    {
        yield return new WaitForSeconds(0f);
        StartCoroutine(FadingIn());
        Debug.Log("Fadein");
    }
    public IEnumerator TestFadeOut()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadingOut());
        Debug.Log("FadeOut");
    }

    public IEnumerator Kill()
    {
        StartCoroutine(TestFadeIn());
        yield return new WaitForSeconds(1f);
        player_Checkpoint.teleportToCheckpoint(player_Checkpoint.currentCheckpoint);
        StartCoroutine(TestFadeOut());
    }

    public IEnumerator Teleport(Transform nextRoom)
    {
        StartCoroutine(TestFadeIn());
        yield return new WaitForSeconds(1f);
        player.transform.position = nextRoom.transform.position;
        StartCoroutine(TestFadeOut());
    }
}
