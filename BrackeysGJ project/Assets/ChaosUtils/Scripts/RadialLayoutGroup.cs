using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[ExecuteInEditMode]
public class RadialLayoutGroup : MonoBehaviour
{
    [Header("Manual Distribution")]
    [Range(0, 360)]
    public float initAngle;
    [Range(0, 360)]
    public float Spacing;
    public float centerDistance;
    [Header("Auto Distribution")]
    public bool autoDistribute;
    [Range(0,360)]
    public float endAngle;
    [Header("Children")]
    public bool rotateChildren;
    [Range(0, 360)]
    public float childAngle;
    [Header("Auto Size")]
    public bool autoSize;
    public float childWidth;
    public float childHeight;

    private RectTransform rt;
    private RectTransform[] Children;
    // Start is called before the first frame update
    private void OnEnable()
    {
        rt = GetComponent<RectTransform>();
        LayoutUpdate();
        //Children = rt.GetComponentsInChildren<RectTransform>();
        
    }
    private void DisableGroups()
    {
        //temporal fix
        //EditorGUI.BeginDisabledGroup(autoDistribute);
        
        //EditorGUI.EndDisabledGroup();

    }

    private void LayoutUpdate()
    {
        Children = new RectTransform[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            Children[i] = child.GetComponent<RectTransform>();
            i++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        DisableGroups();
        Vector2 center = rt.position;
        if (autoDistribute)
        {
            Spacing = (endAngle - initAngle) / (Children.Length + 1);    
        }
        for (int i = 1; i <= Children.Length; i++)
        {
            Children[i-1].gameObject.SetActive(true);
            float childx = centerDistance * Mathf.Cos((initAngle + Spacing * i) * Mathf.Deg2Rad);
            float childy = centerDistance * Mathf.Sin((initAngle + Spacing * i) * Mathf.Deg2Rad);
            Vector2 childpos = center + new Vector2(childx, childy);
            Children[i-1].position = childpos;
            if (rotateChildren)
            {
                Children[i-1].rotation = Quaternion.Euler(0f, 0f, initAngle + Spacing * i + childAngle - 90);
            }
            if (autoSize)
            {
                //Children[i-1].sizeDelta = new Vector2(childWidth, childHeight);
                Children[i - 1].transform.localScale = new Vector2(childWidth, childHeight);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rt.position, centerDistance);
    }
}
