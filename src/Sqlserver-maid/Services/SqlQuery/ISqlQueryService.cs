namespace Sqlserver.maid.Services.SqlQuery
{
    public interface ISqlQueryService
    {
        string Sanitize(string content, string columns);
    }
}