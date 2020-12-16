namespace FileWatcherService
{
    public interface IConfigParser<out T>
    {
        T Parse();
    }
}
