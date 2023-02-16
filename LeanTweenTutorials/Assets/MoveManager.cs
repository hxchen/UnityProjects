using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public GameObject circle;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 point1 = new Vector3(0f, 0f, 0f);
        Vector3 point2 = new Vector3(1f, 0f, 0f);
        Vector3 point3 = new Vector3(2f, 0f, 0f);
        Vector3 point4 = new Vector3(3f, 0f, 0f);
        Vector3 point5 = new Vector3(4f, 0f, 0f);
        Vector3 point6 = new Vector3(5f, 0f, 0f);
        Vector3 point7 = new Vector3(6f, 0f, 0f);

        Vector3[] path1 = {point1, point2, point3, point4, point5, point6, point7};

        Vector3[] path2 = { point1, point2, point3, point4};

        Debug.Log("初始位置：" + circle.transform.position);
        var tween = LeanTween.moveSpline(circle, path2, 2f);
        tween.setEase(LeanTweenType.easeInQuad);
        tween.setOnComplete(() => {
            Debug.Log("最后位置：" + circle.transform.position);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
