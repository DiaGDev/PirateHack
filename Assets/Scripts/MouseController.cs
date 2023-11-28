using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible= false;
    }
    void Update()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));

    }
}
