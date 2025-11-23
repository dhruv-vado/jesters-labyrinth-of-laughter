using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyDetection))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyDetection fov = (EnemyDetection)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360f, fov._radius);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360f, fov._touchDetectionRange);
        
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov._detectionAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov._detectionAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov._radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov._radius);

        if (fov._canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov._player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
