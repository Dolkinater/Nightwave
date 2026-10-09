using UnityEngine;

public class RogStasisTempFix : MonoBehaviour
{
    // the Rog rig moves apart from the character model, so this is a temporary fix to keep him in place

    [SerializeField] Vector3 localPos;

    public void SetLocalPos(Vector3 value) { localPos = value; }
    public Vector3 GetLocalPos() { return localPos; }

    private void Awake()
    {
        localPos = transform.localPosition;
    }

    void SetLocalTransform()
    {
        transform.localPosition = localPos;
    }

    private void LateUpdate()
    {
        SetLocalTransform();
    }
}
