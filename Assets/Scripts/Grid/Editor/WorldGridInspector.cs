using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CustomEditor(typeof(GridManager))]
public class WorldGridInspector : Editor
{

    public VisualTreeAsset inspectorUXML;
    public Button generate;

    public override VisualElement CreateInspectorGUI()
    {
        // Create a new VisualElement to be the root of our inspector UI
        VisualElement myInspector = new VisualElement();

        // Load from default reference
        inspectorUXML.CloneTree(myInspector);
        generate = myInspector.Q<Button>("Generate");
        generate.RegisterCallback<MouseUpEvent>((evt) => target.GetComponent<GridManager>().Generate());

        // Return the finished inspector UI
        return myInspector;
    }
}