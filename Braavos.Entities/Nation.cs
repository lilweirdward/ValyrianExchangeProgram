using System;

namespace Braavos.Entities
{
    public class Nation
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Ruler { get; set; }
        //public Alliance Alliance { get; set; }
        public Account Account { get; set; }
    }
}
