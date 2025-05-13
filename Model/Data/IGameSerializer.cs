using Model.Core;

namespace Model.Data {
    public interface IGameSerializer {
        void SaveGame(GameEngine engine, string filePath);
        GameEngine LoadGame(string filePath, int screenWidth = 480, int screenHeight = 720);
    }
}