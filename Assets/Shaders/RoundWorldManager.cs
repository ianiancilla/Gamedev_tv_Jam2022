using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class RoundWorldManager : MonoBehaviour
{
    #region Constants

    private const string ENABLE_ROUNDING = "_ENABLEROUNDING";

    private static readonly int ROUDING_AMOUNT =
      Shader.PropertyToID("_RoundingAmount");

    #endregion


    #region Inspector

    [SerializeField]
    [Range(0.0005f, 0.1f)]
    private float roundingAmount = 0.005f;

    [SerializeField] private bool roundingEnabled;

    #endregion


    #region Fields

    private float _prevAmount = 0f;

    #endregion


    #region MonoBehaviour

    private void Awake()
    {
        if (Application.isPlaying)
            Shader.EnableKeyword(ENABLE_ROUNDING);
        else
            Shader.DisableKeyword(ENABLE_ROUNDING);


        UpdateBendingAmount();
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
            return;

        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void Update()
    {
        if (Math.Abs(_prevAmount - roundingAmount) > Mathf.Epsilon)
            UpdateBendingAmount();

        if (!Application.isPlaying)
        {
            if (roundingEnabled)
                Shader.EnableKeyword(ENABLE_ROUNDING);
            else
                Shader.DisableKeyword(ENABLE_ROUNDING);

        }
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    #endregion


    #region Methods

    private void UpdateBendingAmount()
    {
        _prevAmount = roundingAmount;
        Shader.SetGlobalFloat(ROUDING_AMOUNT, roundingAmount);
    }

    private static void OnBeginCameraRendering(ScriptableRenderContext ctx,
                                                Camera cam)
    {
        //cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) *
        //                    cam.worldToCameraMatrix;
    }

    private static void OnEndCameraRendering(ScriptableRenderContext ctx,
                                              Camera cam)
    {
        //cam.ResetCullingMatrix();
    }

    #endregion
}