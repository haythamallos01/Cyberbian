namespace CyberbianSite.Client.Models.DID.Talks.Request
{

    public class DIDTalksRequest
    {
        public string source_url { get; set; }
        public Script script { get; set; }
        public Config config { get; set; }
    }

    public class Script
    {
        public string type { get; set; } = "text";
        public string input { get; set; }
    }

    public class Config
    {
        public Driver_Expressions driver_expressions { get; set; }
    }

    public class Driver_Expressions
    {
        public Expression[] expressions { get; set; }
    }

    public class Expression
    {
        public int start_frame { get; set; } = 0;
        public string expression { get; set; } = "surprise";
        public int intensity { get; set; } = 1;
    }
}
