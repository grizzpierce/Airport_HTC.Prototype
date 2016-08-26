﻿using UnityEngine;
using System.Collections;

public class DemoEnd : MonoBehaviour {

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _spawnPoint;
    [SerializeField]
    private TextMesh _text;
    [SerializeField]
    private string _initialText = "Thank you for playing";
    [SerializeField]
    private float _textFadeDuration = 2;
    [SerializeField]
    private float _logoFadeDuration = 2;
    [SerializeField]
    private float _waitDuration = 3;
    [SerializeField]
    private SpriteRenderer _logo;

    private Material _textMaterial;
    void Awake()
    {
        _textMaterial = _text.GetComponent<MeshRenderer>().material;
    }

    public void EndDemo()
    {
        StartCoroutine(EndDemo_Coroutine());
    }

    private IEnumerator EndDemo_Coroutine()
    {
        //go to black
        SteamVR_Fade.Start(Color.black, 1);
        yield return new WaitForSeconds(1);

        //move player
        _player.transform.position = _spawnPoint.transform.position;

        //go to clear
        SteamVR_Fade.Start(Color.clear, 1);
        yield return new WaitForSeconds(1);

        //fade in text
        yield return FadeText(1.0f, _textFadeDuration);
        yield return new WaitForSeconds(3);
        //fade out text
        yield return FadeText(0.0f, _textFadeDuration);

        yield return new WaitForSeconds(1);

        //fade in logo
        yield return FadeLogo(1.0f, _logoFadeDuration);
        //wait 2 seconds
        yield return new WaitForSeconds(5);
        //fade out logo
        yield return FadeLogo(0.0f, _logoFadeDuration);

    }
    IEnumerator FadeText(float aValue, float aTime)
    {
        float alpha = _textMaterial.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(_textMaterial.color.r, _textMaterial.color.g, _textMaterial.color.b, Mathf.Lerp(alpha, aValue, t));
            _textMaterial.color = newColor;
            yield return null;
        }
        Color finalColor = new Color(_textMaterial.color.r, _textMaterial.color.g, _textMaterial.color.b, aValue);
        _textMaterial.color = finalColor;
    }
    IEnumerator FadeLogo(float aValue, float aTime)
    {
        float alpha = _logo.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(_logo.material.color.r, _logo.material.color.g, _logo.material.color.b, Mathf.Lerp(alpha, aValue, t));
            _logo.material.color = newColor;
            yield return null;
        }
        Color finalColor = new Color(_logo.material.color.r, _logo.material.color.g, _logo.material.color.b, aValue);
        _logo.material.color = finalColor;
    }


}