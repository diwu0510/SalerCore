using System.Collections.Generic;

namespace HZC.Core
{
    public class TreeModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string ParentId { get; set; }

        public int Level { get; set; }

        public bool IsLeaf { get; set; }

        public IEnumerable<TreeModel> Children { get; set; }
    }
}
