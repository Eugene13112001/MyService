namespace MyService.Models
{
    public class DataOptions
    {
       
            public string ConnectionString { get; set; } = null!;

            public string DatabaseName { get; set; } = null!;

            public string BooksCollectionName { get; set; } = null!;
        
    }
}
