namespace Sqlserver.maid.Infrastructures
{
    public interface ISqlQueryService
    {
        string Sanitize(string content, string columns);
    }
}