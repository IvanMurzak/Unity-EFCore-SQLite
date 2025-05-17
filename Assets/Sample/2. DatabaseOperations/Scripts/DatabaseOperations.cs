using System.Linq;
using UnityEngine;

public class DatabaseOperations : MonoBehaviour
{
    void Awake()
    {
        AddLevel("Level 1", 1, "Easy level");
        AddLevel("Level 2", 2, "Medium level");
        AddLevel("Level 3", 3, "Hard level");

        PrintAllLevels();
    }

    void AddLevel(string name, int difficulty, string description)
    {
        using (var dbContext = new SQLiteContextFactory().CreateDbContext())
        {
            var level = new LevelData
            {
                Name = name,
                Difficulty = difficulty,
                Description = description
            };
            dbContext.Levels.Add(level);
            dbContext.SaveChanges();
        }
        Debug.Log($"Added Level: {name}, Difficulty: {difficulty}");
    }
    void PrintAllLevels()
    {
        using (var dbContext = new SQLiteContextFactory().CreateDbContext())
        {
            var levels = dbContext.Levels.ToList();
            foreach (var level in levels)
                Debug.Log($"Level ID: {level.Id}, Name: {level.Name}, Difficulty: {level.Difficulty}");
        }
    }
}
