using Naninovel;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InteractableCharacterClickHandler : MonoBehaviour
{
    private string scriptName;
    private BoxCollider2D boxCollider;
    private MeshFilter meshFilter;
    private TransitionalSpriteRenderer spriteRenderer;
    private ICameraManager cameraManager;
    private IScriptPlayer scriptPlayer;

    public void Initialize (string actorId)
    {
        scriptName = actorId switch
        {
            "Npc" => "NpcInteract",
            "Safe" => "SafeInteract",
            "Key" => "KeyInteract",
            _ => string.Empty
        };
    }

    private void Awake ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        meshFilter = GetComponent<MeshFilter>();
        spriteRenderer = GetComponent<TransitionalSpriteRenderer>();
    }

    private void Update ()
    {
        if (!Engine.Initialized)
        {
            if (boxCollider) boxCollider.enabled = false;
            return;
        }

        cameraManager ??= Engine.GetService<ICameraManager>();
        scriptPlayer ??= Engine.GetService<IScriptPlayer>();

        RefreshCollider();

        if (string.IsNullOrWhiteSpace(scriptName))
            return;

        if (scriptPlayer == null)
            return;

        var hasClick = false;
        var screenPosition = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            hasClick = true;
            screenPosition = Mouse.current.position.ReadValue();
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            hasClick = true;
            screenPosition = Input.mousePosition;
        }
#endif

        if (!hasClick) return;

        if (scriptPlayer.Playing)
            return;

        if (cameraManager?.Camera == null)
            return;

        if (boxCollider == null)
            return;

        if (!boxCollider.enabled)
            return;

        var worldPoint = cameraManager.Camera.ScreenToWorldPoint(screenPosition);
        var hit = Physics2D.OverlapPoint(new Vector2(worldPoint.x, worldPoint.y));

        if (hit == boxCollider)
            scriptPlayer.PreloadAndPlayAsync(scriptName).Forget();
    }

    private void RefreshCollider ()
    {
        if (boxCollider == null || spriteRenderer == null)
            return;

        var hasMeshBounds = meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.bounds.size.sqrMagnitude > 0.0001f;
        if (hasMeshBounds)
        {
            var bounds = meshFilter.sharedMesh.bounds;
            boxCollider.offset = bounds.center;
            boxCollider.size = bounds.size;
        }
        else if (spriteRenderer.MainTexture != null && spriteRenderer.PixelsPerUnit > 0)
        {
            var width = spriteRenderer.MainTexture.width / spriteRenderer.PixelsPerUnit;
            var height = spriteRenderer.MainTexture.height / spriteRenderer.PixelsPerUnit;
            var pivot = spriteRenderer.Pivot;
            boxCollider.size = new Vector2(width, height);
            boxCollider.offset = new Vector2((0.5f - pivot.x) * width, (0.5f - pivot.y) * height);
        }

        boxCollider.enabled = spriteRenderer.Opacity > 0.01f;
    }
}
