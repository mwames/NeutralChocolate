using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Tiled;

namespace NeutralChocolate {
    public static class EntityFactory {
        public static List<IEntity> ReadMapLayer(TiledMap map, string layerName) {
            return map.GetLayer<TiledMapObjectLayer>(layerName).Objects
                .Select(enemy => EntityFactory.Create(enemy))
                .Where((enemy) => enemy != null)
                .ToList();
        }
        public static IEntity Create(TiledMapObject tmo)
        {
            string type;
            tmo.Properties.TryGetValue("Type", out type);

            switch (type)
            {
                case "Snake":
                    return new Snake(tmo.Position);
                case "Eye":
                    return new Eye(tmo.Position);
                case "Tree":
                    return new Tree(tmo.Position);
                case "Bush":
                    return new Bush(tmo.Position);
                default:
                    return null;
            }
        }
    }
}
