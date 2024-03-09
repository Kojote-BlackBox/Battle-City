using UnityEngine;
using UnityEngine.AI;
using World;

public class Camera : MonoBehaviour
{
    #region follow
    [Header("Follow")]
    public GameObject gameObjectToFollow;
    #endregion

    #region restriction
    [Header("Restriction")]
    public Map map;

    public Vector2 offsetLimitX = new Vector2(-.5f, -.5f);
    public Vector2 offsetLimitY = new Vector2(-.5f, -.5f);
    #endregion

    #region camera
    private UnityEngine.Camera _camera;
    private float _sizeVerticalHalf = 0f;
    private float _sizeHorizontalHalf = 0f;
    #endregion

    void Awake()
    {
        _camera = UnityEngine.Camera.main;

        _sizeHorizontalHalf = _camera.orthographicSize * Screen.width / Screen.height;
        _sizeVerticalHalf = _camera.orthographicSize;
    }

    void Update()
    {
        if (gameObjectToFollow != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp(gameObjectToFollow.transform.position.x, _sizeHorizontalHalf + offsetLimitX.x, map.columns - _sizeHorizontalHalf + offsetLimitX.y),
                Mathf.Clamp(gameObjectToFollow.transform.position.y, _sizeVerticalHalf + offsetLimitY.x, map.rows - _sizeVerticalHalf + offsetLimitY.y),
                Mathf.Clamp(gameObjectToFollow.transform.position.z, -5f, -5f)
            );
        }
    }
}
