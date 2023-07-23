namespace CyberbianSite.Client.Models.DID.Clips
{
    public class DIDClipsActorsResponse
    {
        public Actor[] actors { get; set; }
    }

    public class Actor
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public string thumbnail_url { get; set; }
        public string image_url { get; set; }
        public string gender { get; set; }
        public DateTime modified_at { get; set; }
    }
}
