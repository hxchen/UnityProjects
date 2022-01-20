using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OKButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("鼠标按下");
        transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("鼠标抬起");
        transform.localScale = new Vector3(1, 1, 1);
    }
}
