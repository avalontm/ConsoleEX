namespace ConsoleEX
{
    public class ConsoleMenuItem
    {
        public string? Title { get; set; }
        public Func<Task>? Action { get; set; }
    }
}
