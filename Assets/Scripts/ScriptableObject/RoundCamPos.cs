using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Classe usada para corrigir erro na posição da camera
/// </summary>
public class RoundCamPos : CinemachineExtension
{
    public float PixelsPerUnit = 32; // quantidade de pixels por unidade

    /*correção no posicionamento da camera
    */
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float DeltaTime)
    {
        if(stage == CinemachineCore.Stage.Body){
            Vector3 pos = state.FinalPosition;
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);
            state.PositionCorrection += pos2 - pos;
        }
    }

    /*arredondamento
    */
    float Round(float x){
        return Mathf.Round(x*PixelsPerUnit) / PixelsPerUnit; 
    }
}
