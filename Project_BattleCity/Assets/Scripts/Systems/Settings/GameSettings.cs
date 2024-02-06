[System.Serializable]
public class GameSettings {
    public AudioSettings audioSettings = new AudioSettings();
    public GraphicsSettings graphicsSettings = new GraphicsSettings();
    public GameplaySettings gameplaySettings = new GameplaySettings();
    public ControlSettings controlSettings = new ControlSettings();
    public ControlSettings defaultControlSettings = new ControlSettings();
}
