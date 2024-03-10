using UnityEngine;
using UnityEngine.UI;

public class Indicator : Image
{

    public void SetState( bool enabled ) {

        if ( enabled ) Show();
        else Hide();
    }


    public void Show() { gameObject.SetActive( true ); }
    public void Hide() { gameObject.SetActive( false ); }



    public void SetParent( Transform parent ) {

        rectTransform.SetParent( parent, false );
    } 


    public Vector3 position
    {
        get { return rectTransform.localPosition; }
        set { rectTransform.localPosition = value; }
    }


    public Quaternion rotation
    {
        get { return rectTransform.localRotation; }
        set { rectTransform.localRotation = value; }
    }
}
