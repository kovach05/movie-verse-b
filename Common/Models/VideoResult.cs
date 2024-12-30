namespace Common.Models;

public class VideoResult
{
    public string Id { get; set; }
    public string Iso_639_1 { get; set; }
    public string Iso_3166_1 { get; set; }
    public string Key { get; set; } // Це ключ для YouTube відео
    public string Name { get; set; }
    public string Site { get; set; } // Наприклад, "YouTube"
    public int Size { get; set; } // Наприклад, 1080
    public string Type { get; set; } // Наприклад, "Trailer"
}