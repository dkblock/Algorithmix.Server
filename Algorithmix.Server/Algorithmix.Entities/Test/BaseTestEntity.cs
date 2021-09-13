using System;

namespace Algorithmix.Entities.Test
{
    public class BaseTestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
