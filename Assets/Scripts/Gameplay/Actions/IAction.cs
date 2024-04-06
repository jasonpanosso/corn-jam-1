public interface IAction
{
    public bool HasExecuted { get; set; }
    public void Execute();
}
