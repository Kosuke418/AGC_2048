using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{

    public Text damageText;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 2f;

    void Start()
    {
        damageText = GetComponentInChildren<Text>();
    }

    void LateUpdate()
    {
        damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

        if (damageText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}