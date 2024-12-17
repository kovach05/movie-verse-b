namespace Movie_Verse.DTOs;

public class MovieCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
}