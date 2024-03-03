using UnityEngine;

namespace Gameplay.Projectile
{
    public class ComponentShellShadow : MonoBehaviour
    {
        #region shell
        private ComponentShell _parentShell;
        #endregion

        #region shadow
        private Rigidbody2D _rigidBodyShellShadow;
        #endregion

        void Start()
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.15f);
        }

        public void Initialize(ComponentShell shell)
        {
            _parentShell = shell;
            _rigidBodyShellShadow = GetComponent<Rigidbody2D>();

            gameObject.transform.right = _parentShell.gameObject.transform.right;

            _rigidBodyShellShadow.velocity = _parentShell.directionForward * _parentShell.GetVelocity();
        }

        private void FixedUpdate()
        {
            if (_parentShell == null) return;

            transform.position = new Vector2(transform.position.x, transform.position.y - _parentShell._bulletDropEachFrame);
        }
    }
}
