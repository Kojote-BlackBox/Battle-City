using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlurEffectScript : MonoBehaviour {

    [Range(0, 7)]
    public int iterations = 0;

    void OnRenderImage(RenderTexture src, RenderTexture destination) {
        if (iterations == 0) {
            Graphics.Blit(src, destination);
            return;
        }
  
        int width = src.width / 2;
        int height = src.height / 2;
        RenderTextureFormat format = src.format;

        RenderTexture currentDestination = RenderTexture.GetTemporary(
            width, height, 0, format
        );

        Graphics.Blit(src, currentDestination);
        RenderTexture currentSource = currentDestination;

        for (int i = 1; i < iterations; i++) {
            width /= 2;
            height /= 2;

            if (height < 2) {
                break;
            }

            currentDestination = RenderTexture.GetTemporary(
                width, height, 0, format
            );

            Graphics.Blit(currentSource, currentDestination);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }

        Graphics.Blit(currentSource, destination);
        RenderTexture.ReleaseTemporary(currentSource);
    }
}
