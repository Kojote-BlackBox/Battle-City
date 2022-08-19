using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour {


    [Range(0, 1)]
    public int on = 0;

    public Shader postEffectShader;
    public Material mat;


    // src is the full rendered scene that you would normaly
    // send directly to the monitor. We are intercepting
    // this so we can do a bit more work, before passing it on.
    void OnRenderImage(RenderTexture src, RenderTexture dest) {

        if(on == 0) {
            Graphics.Blit(src, dest);
            return;
        }

        if (mat == null) {
            mat = new Material(postEffectShader);
            mat.hideFlags = HideFlags.HideAndDontSave;
        }

        // copy src into the dests
        Graphics.Blit(src, dest, mat);
    }
}
