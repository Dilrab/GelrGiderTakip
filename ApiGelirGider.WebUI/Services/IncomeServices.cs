public class IncomeService
{
    private readonly HttpClient _httpClient;

    public IncomeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

}
