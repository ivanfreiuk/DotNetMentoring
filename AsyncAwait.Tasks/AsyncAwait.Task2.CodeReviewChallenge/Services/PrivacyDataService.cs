using System.Text;

namespace AsyncAwait.Task2.CodeReviewChallenge.Services;

public class PrivacyDataService : IPrivacyDataService
{
    public string GetPrivacyData()
    {
        return new StringBuilder()
            .Append("This Policy describes how async/await processes your personal data, ")
            .Append("but it may not address all possible data processing scenarios.")
            .ToString();
    }
}
