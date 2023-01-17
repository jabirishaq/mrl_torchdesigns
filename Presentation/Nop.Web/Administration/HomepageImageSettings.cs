
using Nop.Core.Configuration;


namespace Nop.Admin
{
    public class HomepageImageSettings : ISettings
    {
        public int Picture1Id { get; set; }
        public string Link1 { get; set; }

        public int Picture2Id { get; set; }
        public string Link2 { get; set; }

    }
}