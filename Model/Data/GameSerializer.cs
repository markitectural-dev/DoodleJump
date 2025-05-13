using Model.Core;

namespace Model.Data {
    public abstract class GameSerializer {
        public abstract void SaveGame(GameEngine engine, string filePath);

        public abstract GameEngine LoadGame(string filePath, int screenWidth = 480, int screenHeight = 720);

        public static T GetSerializer<T>(SerializerType type) where T : GameSerializer { // QUESTION: necessary?
            switch (type) {
                case SerializerType.JSON:
                    return new JSONGameSerializer() as T;
                case SerializerType.XML:
                    return new XMLGameSerializer() as T;
                default:
                    return new JSONGameSerializer() as T; 
            }
        }
        
        public enum SerializerType {
            JSON,
            XML
        }
    }
}
