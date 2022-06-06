namespace Sqlserver.maid.Services.File
{
    public static class FileServiceFactory
    {
        public static IFileService GetService(string extension)
        {
            switch (extension)
            {
                case ".xlsx":
                    return new ExcelFileService();

                case ".json":
                    return new JsonFileService();
            }

            return default;
        }
    }
}