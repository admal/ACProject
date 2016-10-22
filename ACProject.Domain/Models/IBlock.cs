namespace ACProject.Domain.Models
{
    public interface IBlock
    {
        int Count { get; set; }
        int[,] Grid { get; set; }
    }
}