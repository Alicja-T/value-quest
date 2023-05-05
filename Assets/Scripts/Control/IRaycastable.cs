namespace RPG.Control {

public interface IRaycastable {
    CursorType GetCursorType();
    bool handleRaycast(PlayerController caller);
}

}
