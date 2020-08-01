using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SceneAutoLoader))]
public class SceneEditor : Editor
{
    SceneAutoLoader controller;
    private void OnEnable()
    {
        controller = (SceneAutoLoader)target;
    }

    public void OnSceneGUI()
    {
        if (!controller.edit) return;
        for(int i = 0; i < controller.SceneBounds.Length; i++)
        {
            Handles.color = Color.yellow;
            controller.SceneBounds[i].center = Handles.FreeMoveHandle(controller.SceneBounds[i].center, Quaternion.identity, HandleUtility.GetHandleSize(controller.SceneBounds[i].center) * 0.4f, Vector3.one, Handles.CubeHandleCap);
            Handles.color = Color.green;
            float boundx = Handles.FreeMoveHandle(controller.SceneBounds[i].center + new Vector3(controller.SceneBounds[i].extents.x,0f,0f), Quaternion.identity, HandleUtility.GetHandleSize(controller.SceneBounds[i].center + new Vector3(controller.SceneBounds[i].extents.x, 0f, 0f))*0.14f, Vector3.one, Handles.CubeHandleCap).x;
            boundx -= controller.SceneBounds[i].center.x;
            float boundy = Handles.FreeMoveHandle(controller.SceneBounds[i].center + new Vector3(0f,controller.SceneBounds[i].extents.y, 0f), Quaternion.identity, HandleUtility.GetHandleSize(controller.SceneBounds[i].center + new Vector3(controller.SceneBounds[i].extents.x, 0f, 0f))*0.14f, Vector3.one, Handles.CubeHandleCap).y;
            boundy -= controller.SceneBounds[i].center.y;
            controller.SceneBounds[i].extents = new Vector3(boundx, boundy, 1f);
            try
            {
                Handles.Label(controller.SceneBounds[i].center, ""+i+":"+controller.SceneNames[i]);
            }
            catch {
                Handles.Label(controller.SceneBounds[i].center, "" + i);
            };
        }
        //Load
        //Handles.color = Color.blue;
        //controller.LoadBounds.center = Handles.FreeMoveHandle(controller.LoadBounds.center, Quaternion.identity, HandleUtility.GetHandleSize(controller.LoadBounds.center) * 0.4f, Vector3.one, Handles.CubeHandleCap);
        Handles.color = Color.cyan;
        float lboundx = Handles.FreeMoveHandle(controller.LoadBounds.center + new Vector3(controller.LoadBounds.extents.x, 0f, 0f), Quaternion.identity, HandleUtility.GetHandleSize(controller.LoadBounds.center + new Vector3(controller.LoadBounds.extents.x, 0f, 0f)) * 0.14f, Vector3.one, Handles.CubeHandleCap).x;
        lboundx -= controller.LoadBounds.center.x;
        float lboundy = Handles.FreeMoveHandle(controller.LoadBounds.center + new Vector3(0f, controller.LoadBounds.extents.y, 0f), Quaternion.identity, HandleUtility.GetHandleSize(controller.LoadBounds.center + new Vector3(controller.LoadBounds.extents.x, 0f, 0f)) * 0.14f, Vector3.one, Handles.CubeHandleCap).y;
        lboundy -= controller.LoadBounds.center.y;
        controller.LoadBounds.extents = new Vector3(lboundx, lboundy, 1f);
        Handles.Label(controller.LoadBounds.center, "Loading Zone");
        //Unload
        //Handles.color = Color.magenta;
        //controller.UnLoadBounds.center = Handles.FreeMoveHandle(controller.UnLoadBounds.center, Quaternion.identity, HandleUtility.GetHandleSize(controller.UnLoadBounds.center) * 0.4f, Vector3.one, Handles.CubeHandleCap);
        Handles.color = Color.red;
        float unboundx = Handles.FreeMoveHandle(controller.UnLoadBounds.center + new Vector3(controller.UnLoadBounds.extents.x, 0f, 0f), Quaternion.identity, HandleUtility.GetHandleSize(controller.UnLoadBounds.center + new Vector3(controller.UnLoadBounds.extents.x, 0f, 0f)) * 0.14f, Vector3.one, Handles.CubeHandleCap).x;
        unboundx -= controller.UnLoadBounds.center.x;
        float unboundy = Handles.FreeMoveHandle(controller.UnLoadBounds.center + new Vector3(0f, controller.UnLoadBounds.extents.y, 0f), Quaternion.identity, HandleUtility.GetHandleSize(controller.UnLoadBounds.center + new Vector3(controller.UnLoadBounds.extents.x, 0f, 0f)) * 0.14f, Vector3.one, Handles.CubeHandleCap).y;
        unboundy -= controller.UnLoadBounds.center.y;
        controller.UnLoadBounds.extents = new Vector3(unboundx, unboundy, 1f);
        Handles.Label(controller.UnLoadBounds.center+Vector3.up*2f, "Unloading Zone");

    }
}
