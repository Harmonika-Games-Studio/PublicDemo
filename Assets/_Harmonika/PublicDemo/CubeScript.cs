using System.Collections;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private Renderer _cubeRenderer;
    private Color _targetColor;
    private Coroutine _colourRoutine;

    void Start()
    {
        _cubeRenderer = GetComponent<Renderer>();
    }

    void OnMouseDown()
    {
        ChangeToRandomColor();
    }

    void OnMouseUpAsButton()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            ChangeToRandomColor();
        }
    }

    void ChangeToRandomColor()
    {
        _targetColor = new Color(Random.value, Random.value, Random.value);

        if (_colourRoutine != null) return;


        _colourRoutine = StartCoroutine(FadeToColor(_targetColor));
    }

    IEnumerator FadeToColor(Color newColor, float fadeSpeed = 2f)
    {
        Color currentColor = _cubeRenderer.material.color;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * fadeSpeed;
            _cubeRenderer.material.color = Color.Lerp(currentColor, newColor, progress);
            yield return null;
        }

        _cubeRenderer.material.color = newColor;
        _colourRoutine = null;
    }

    void OnTouch()
    {
        ChangeToRandomColor();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Apenas para fins de demonstração, se precisar de interações por colisão
    }
}
