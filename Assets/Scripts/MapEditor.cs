using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (TileMapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI(){
       // base.OnInspectorGUI();

        TileMapGenerator map = target as TileMapGenerator;

        if(DrawDefaultInspector()){
            map.GenerateMap();
        }

        if(GUILayout.Button("Generate Map")){
            map.GenerateMap();
        }
    }
}
