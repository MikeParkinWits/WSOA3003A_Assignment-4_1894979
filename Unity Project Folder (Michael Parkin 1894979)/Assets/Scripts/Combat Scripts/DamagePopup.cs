using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{

    private TextMeshPro textMesh;

    private float dissapearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount)
    {

        if (damageAmount == -1)
        {
            textMesh.SetText("Missed");
        }
        else
        {
            textMesh.SetText(damageAmount.ToString());
        }

        textColor = textMesh.color;
        dissapearTimer = 1f;
    }

    private void Update()
    {
        float yMoveSpeed = 2f;
        transform.position += new Vector3(0, yMoveSpeed) * Time.deltaTime;

        dissapearTimer -= Time.deltaTime;

        if (dissapearTimer < 0)
        {

            float dissapearSpeed = 6f;

            textColor.a -= dissapearSpeed * Time.deltaTime;

            textMesh.color = textColor;
        }

        if (textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }


}
