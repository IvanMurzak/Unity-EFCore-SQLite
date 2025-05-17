using System.ComponentModel.DataAnnotations;

public class LevelData
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    [Range(1, 10)]
    public int Difficulty { get; set; }
    public string Description { get; set; }
}