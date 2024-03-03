using UnityEngine;
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
    #endregion

    void Update()
    {
        if (gameObjectToFollow != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp(gameObjectToFollow.transform.position.x, 0f, map.rows),
                Mathf.Clamp(gameObjectToFollow.transform.position.y, 0f, map.columns),
                Mathf.Clamp(gameObjectToFollow.transform.position.z, -5f, -5f)
            );
        }
    }
}
