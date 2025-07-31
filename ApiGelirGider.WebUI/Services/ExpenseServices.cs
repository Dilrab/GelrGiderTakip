
public class ExpenseService
{
    private readonly HttpClient _httpClient;

    public ExpenseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

   
}
