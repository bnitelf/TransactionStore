using TransactionStore.Services.FileIO;

namespace TransactionStore.Factory
{
    public class FileParserFactory
    {
        public IFileParser GetFileParser(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".csv":
                    return new CsvFileParser();
                case ".xml":
                    return new XmlFileParser();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
