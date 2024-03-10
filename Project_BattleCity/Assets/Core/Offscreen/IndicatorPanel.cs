using System.Collections.Generic;
using UnityEngine;

public class IndicatorPanel : MonoBehaviour
{
    private static IndicatorPanel instance;

    [SerializeField]
    private UnityEngine.Camera _camera;

    private List<Transform> objects = new List<Transform>();

    [SerializeField]
    private RectTransform containerIndicators;

    [SerializeField]
    private Indicator sampleIndicator;

    private List<Indicator> indicators = new List<Indicator>();

    public void Init() {

        instance = this;
        UpdateScreenParams();
        InitNewIndicators( 5 );

        foreach ( Indicator indicator in indicators )
            indicator.Hide( );
    }

    public static void AddObject( Transform obj ) {

        if ( instance != null ) instance.AddObj( obj );
    }


    private void AddObj( Transform obj ) {

        objects.Add( obj );

        if ( indicators.Count < objects.Count )
            InitNewIndicators( objects.Count - indicators.Count );

        UpdateStateObjects();
    }


    private void InitNewIndicators(int count) {

        for( int i = 0; i < count; i++ ) {

            Indicator indicator = Instantiate<Indicator>( sampleIndicator );
            indicator.SetParent( containerIndicators );
            indicators.Add( indicator );
        }

    }

    public static void RemoveObject( Transform obj ) {

        if ( instance != null ) instance.RemoveObj( obj );
    }


    private void RemoveObj( Transform obj ) {

        objects.Remove( obj );
        UpdateStateObjects();
    }


    private int k;

    private void UpdateStateObjects() {

        for( k = 0; k < indicators.Count; k++ ) 
            indicators[k].SetState( k < objects.Count );
    }


    private void UpdateScreenParams() {

        oldScreenW = Screen.width;
        oldScreenH = Screen.height;

        screenCenter = new Vector3( Screen.width / 2.0f, Screen.height / 2.0f, 0.0f );
        halfContainerSize =containerIndicators.rect.size / 2.0f;
    }


    private int i = 0;
    private Vector3 screenPosObj;
    private Vector3 screenCenter;
    private Vector3 halfContainerSize;

    private float oldScreenW;
    private float oldScreenH;

    private float angle;

    private Vector3 posIndicator = Vector3.zero;
    private Vector3 rotIndicator = Vector3.zero;


    private void LateUpdate() {

        if ( objects.Count == 0 )
            return;


        if ( oldScreenH != Screen.height || oldScreenW != Screen.width )
            UpdateScreenParams();


        for ( i = 0; i < objects.Count; i++ ) {

            screenPosObj = _camera.WorldToScreenPoint( objects[i].transform.position );

            if( screenPosObj.x < 0 || screenPosObj.x > Screen.width || screenPosObj.y < 0 || screenPosObj.y > Screen.height ) {

                //Show the current indicator
                indicators[i].SetState( true );

                angle = Mathf.Atan2( screenPosObj.y - halfContainerSize.y, screenPosObj.x - halfContainerSize.x );

                posIndicator.x =  Mathf.Cos( angle ) * halfContainerSize.x;
                posIndicator.y =  Mathf.Sin( angle ) * halfContainerSize.y;
                posIndicator.z =  0.0f;

                indicators[i].position = posIndicator;

                rotIndicator = indicators[i].rotation.eulerAngles;
                rotIndicator.z = angle * Mathf.Rad2Deg;

                indicators[i].rotation = Quaternion.Euler( rotIndicator );

            } else {

                //Hide the current indicator
                indicators[i].SetState( false );
            }

        }
        
    }
}
