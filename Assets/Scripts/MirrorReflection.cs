using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private Transform mirrorSurface;
    [SerializeField] private Camera mirrorCamera;

    void Update()
    {
        // Définir le point de départ du Raycast (position de MirrorCameraPlace)
        Vector3 rayOrigin = raycastOrigin.position;
        Vector3 rayDirection = raycastOrigin.forward; // Direction du regard

        // Définir le plan du miroir
        Vector3 mirrorPos = mirrorSurface.position;
        Vector3 mirrorNormal = mirrorSurface.forward;

        // Calculer l'intersection du Raycast avec le plan du miroir
        Vector3 cameraToMirror = rayOrigin - mirrorPos;
        float dot = Vector3.Dot(cameraToMirror, mirrorNormal);
        float rayDot = Vector3.Dot(rayDirection, mirrorNormal);


        Vector3 hitPoint = rayOrigin + rayDirection;
        mirrorCamera.transform.position = hitPoint;


        // // Calculer la direction réfléchie
        // Vector3 lookDir = raycastOrigin.forward;
        // Vector3 reflectedLookDir = lookDir - 2f * Vector3.Dot(lookDir, mirrorNormal) * mirrorNormal;

        // // Appliquer la rotation réfléchie à la caméra
        // mirrorCamera.transform.forward = reflectedLookDir;
    }
}
