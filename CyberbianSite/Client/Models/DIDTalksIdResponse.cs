namespace CyberbianSite.Client.Models.DID.Talks.Response
{
    public class DIDTalksIdResponse
    {
        public User user { get; set; }
        public Script script { get; set; }
        public Metadata metadata { get; set; }
        public string audio_url { get; set; }
        public DateTime created_at { get; set; }
        public Face face { get; set; }
        public Config config { get; set; }
        public string source_url { get; set; }
        public string created_by { get; set; }
        public string status { get; set; }
        public string driver_url { get; set; }
        public DateTime modified_at { get; set; }
        public string user_id { get; set; }
        public bool subtitles { get; set; }
        public string id { get; set; }
        public float duration { get; set; }
        public DateTime started_at { get; set; }
        public string result_url { get; set; }
    }

    public class User
    {
        public string[] features { get; set; }
        public string id { get; set; }
        public string plan { get; set; }
        public string authorizer { get; set; }
        public string email { get; set; }
        public string owner_id { get; set; }
    }

    public class Script
    {
        public string type { get; set; }
        public bool ssml { get; set; }
        public bool subtitles { get; set; }
    }

    public class Metadata
    {
        public string driver_url { get; set; }
        public bool mouth_open { get; set; }
        public int num_faces { get; set; }
        public int num_frames { get; set; }
        public float processing_fps { get; set; }
        public int[] resolution { get; set; }
        public float size_kib { get; set; }
    }

    public class Face
    {
        public int mask_confidence { get; set; }
        public int[] detection { get; set; }
        public string overlap { get; set; }
        public int size { get; set; }
        public int[] top_left { get; set; }
        public int face_id { get; set; }
        public float detect_confidence { get; set; }
    }

    public class Config
    {
        public bool stitch { get; set; }
        public bool align_driver { get; set; }
        public bool sharpen { get; set; }
        public int normalization_factor { get; set; }
        public string result_format { get; set; }
        public bool fluent { get; set; }
        public Driver_Expressions driver_expressions { get; set; }
        public int pad_audio { get; set; }
        public bool reduce_noise { get; set; }
        public bool auto_match { get; set; }
        public Logo logo { get; set; }
        public int motion_factor { get; set; }
        public float align_expand_factor { get; set; }
    }

    public class Driver_Expressions
    {
        public Expression[] expressions { get; set; }
        public int transition_frames { get; set; }
    }

    public class Expression
    {
        public int intensity { get; set; }
        public int start_frame { get; set; }
        public string expression { get; set; }
    }

    public class Logo
    {
        public string url { get; set; }
        public int[] position { get; set; }
    }

}
