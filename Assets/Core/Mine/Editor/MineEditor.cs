using System.Collections.Generic;
using System.Linq;
using Core.Mine.Runtime;
using Core.Mine.Runtime.Point.Base;
using Core.Mine.Runtime.Point.Default;
using Core.Mine.Runtime.Point.GoldResource;
using Core.Mine.Runtime.RouteBuildType.ByPathfinding;
using Core.Mine.Runtime.RouteBuildType.Defined;
using Services.Navigation.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace Core.Mine.Editor
{
    [CustomEditor(typeof(MineConfig))]
    public class MineEditor : UnityEditor.Editor
    {
        private const string c_BasePoint = "base_point";
        private const string c_Waypoint = "waypoint";
        private const string c_ResourcePoint = "resource_point";

        private string m_CurrentState = string.Empty;
        private MineConfig m_MineConfig;
        
        void OnEnable()
        {
            m_MineConfig = (MineConfig)target;
            
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal(); 
            {
                if (GUILayout.Button("Add Base Point"))
                {
                    m_CurrentState = c_BasePoint;
                }

                if (GUILayout.Button("Add Waypoint"))
                {
                    m_CurrentState = c_Waypoint;
                }
            
                if (GUILayout.Button("Add ResourcePoint"))
                {
                    m_CurrentState = c_ResourcePoint;
                }

            } 
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Set Defined Route Build Type"))
                {
                    m_MineConfig.RouteBuildType = new DefinedRouteBuildType();
                }
                
                if (GUILayout.Button("Set Pathfinding Build Type"))
                {
                    m_MineConfig.RouteBuildType = new ByPathfindingBuildType();
                }
            }
            EditorGUILayout.EndHorizontal();

        }

        private void OnSceneGUI(SceneView obj)
        {
            DrawRouteMap();

            if (string.IsNullOrEmpty(m_CurrentState))
            {
                return;
            }
            
            int id = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(id);
            
            var e = Event.current;
            var ray = GetPointerPosition(e.mousePosition);
            DrawWireCube(ray, Vector3.one, Color.red);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                var uniqueId= GetUniqueId();
                switch (m_CurrentState)
                {
                    case c_BasePoint:
                        m_MineConfig.Points.Add(new BasePoint(uniqueId, new int[]{}, ray));
                        break;
                    case c_Waypoint:
                        m_MineConfig.Points.Add(new Waypoint(uniqueId, new int[]{}, ray));
                        break;
                    case c_ResourcePoint:
                        m_MineConfig.Points.Add(new GoldResourcePoint(uniqueId, new int[]{}, ray));
                        break;
                }
            }
            
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                m_CurrentState = string.Empty;
            }
            
            SceneView.RepaintAll();
        }

        private int GetUniqueId()
        {
            if (m_MineConfig.Points.Count == 0)
            {
                return 0;
            }
            
            var maxId=  m_MineConfig.Points.Max(p => p.Id);
            return ++maxId;
        }
        
        private void DrawRouteMap()
        {
            var navigationMap = m_MineConfig.GetMineMap().NavigationMap;
            if (navigationMap == null || !navigationMap.Keys.Any())
            {
                return;
            }

            foreach (var pair in navigationMap)
            {
                var from = pair.Key;
                DrawWireCube(from.Position, Vector3.one, Color.green);

                // from.Id.ToString()v
                var style = new GUIStyle
                {
                    normal = { textColor = Color.red }, 
                    fontSize = 14
                };
                
                Handles.Label(from.Position, from.Id.ToString(), style);
                
                foreach (var to in pair.Value)
                {
                    var direction = NavigationUtils.GetTransitionDirection(to, pair.Key);
                    if (direction.normalized == Vector3.zero)
                    {
                        continue;
                    }
                    
                    float arrowHeadLength = 0.35f;
                    float arrowHeadAngle = 25f;

                    var right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back * arrowHeadLength;
                    var left  = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back * arrowHeadLength;
                    var up    = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back * arrowHeadLength;
                    var down  = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back * arrowHeadLength;

                    var end = to.Position;
                    Handles.DrawLine(end, end + right);
                    Handles.DrawLine(end, end + left);
                    Handles.DrawLine(end, end + up);
                    Handles.DrawLine(end, end + down);
                    
                    Handles.DrawLine(from.Position, to.Position);
                }
            }
        }

        private Vector3 GetPointerPosition(Vector3 mousePosition)
        {
            var worldRay = HandleUtility.GUIPointToWorldRay(mousePosition);
            var ray = new Vector3(worldRay.origin.x, worldRay.origin.y, 0);
            return ray;
        }

        private void DrawWireCube(Vector3 center, Vector3 size, Color color)
        {
            var colorTemp = Handles.color;
            Handles.color = color;
            Handles.DrawWireCube(center, size);
            Handles.color = colorTemp;

        }
        
        private void OnSceneGUI()
        {
            //call me if this asshole works and we'll get to work.
        }

    }
}