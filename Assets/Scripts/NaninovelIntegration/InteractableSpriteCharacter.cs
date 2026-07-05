using Naninovel;
using UnityEngine;

public class InteractableSpriteCharacter : SpriteCharacter
{
    public InteractableSpriteCharacter (string id, CharacterMetadata metadata)
        : base(id, metadata) { }

    public override async UniTask InitializeAsync ()
    {
        await base.InitializeAsync();

        var collider = GameObject.GetComponent<BoxCollider2D>();
        if (collider == null)
            collider = GameObject.AddComponent<BoxCollider2D>();

        collider.isTrigger = true;

        var handler = GameObject.GetComponent<InteractableCharacterClickHandler>();
        if (handler == null)
            handler = GameObject.AddComponent<InteractableCharacterClickHandler>();

        handler.Initialize(Id);
    }
}
