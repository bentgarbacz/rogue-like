using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Utility manager for controlling material transparency on GameObjects.
/// Provides methods to make materials transparent, opaque, and set transparency levels.
/// </summary>
public static class TransparencyManager
{
    /// <summary>
    /// Sets the transparency level of a material on a given renderer.
    /// Automatically configures the material for transparency rendering.
    /// </summary>
    /// <param name="renderer">The Renderer component containing the material</param>
    /// <param name="alpha">Alpha value (0 = fully transparent, 1 = fully opaque)</param>
    public static void SetTransparency(Renderer renderer, float alpha)
    {
        if (renderer == null)
        {
            Debug.LogWarning("TransparencyManager: Renderer is null");
            return;
        }

        alpha = Mathf.Clamp01(alpha); // Ensure alpha is between 0 and 1

        Material newMat = new Material(renderer.material);
        ConfigureForTransparency(newMat);

        Color color = newMat.color;
        color.a = alpha;
        newMat.color = color;

        renderer.material = newMat;
    }

    /// <summary>
    /// Makes a material completely opaque (alpha = 1).
    /// </summary>
    /// <param name="renderer">The Renderer component containing the material</param>
    public static void SetOpaque(Renderer renderer)
    {
        if (renderer == null)
        {
            Debug.LogWarning("TransparencyManager: Renderer is null");
            return;
        }

        Material newMat = new Material(renderer.material);
        ConfigureForOpaque(newMat);

        Color color = newMat.color;
        color.a = 1f;
        newMat.color = color;

        renderer.material = newMat;
    }

    /// <summary>
    /// Configures a material for transparent rendering with proper shader settings and blend modes.
    /// </summary>
    /// <param name="material">The material to configure</param>
    private static void ConfigureForTransparency(Material material)
    {
        material.SetFloat("_Mode", 3f);
        material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }

    /// <summary>
    /// Configures a material for opaque rendering.
    /// </summary>
    /// <param name="material">The material to configure</param>
    private static void ConfigureForOpaque(Material material)
    {
        material.SetFloat("_Mode", 0f);
        material.SetInt("_SrcBlend", (int)BlendMode.One);
        material.SetInt("_DstBlend", (int)BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 2000;
    }
}
